use actix_web::{web, App, HttpResponse, HttpServer, middleware::Logger};
use actix_web::error::{Error, InternalError, JsonPayloadError, ErrorInternalServerError};
use std::env;
use serde::{Deserialize, Serialize};
use chrono::NaiveDateTime;
use mysql::prelude::FromRow;
use mysql::*;
use mysql::prelude::*;
use bcrypt::{verify, DEFAULT_COST};
use std::fmt;
use log;

#[derive(Debug)]
pub enum ApiError {
    DbError(mysql::Error),
    BcryptError(bcrypt::BcryptError),
    ValidationError(String),
}

impl fmt::Display for ApiError {
    fn fmt(&self, f: &mut fmt::Formatter) -> fmt::Result {
        match self {
            ApiError::DbError(e) => write!(f, "Database error: {}", e),
            ApiError::BcryptError(e) => write!(f, "Bcrypt error: {}", e),
            ApiError::ValidationError(e) => write!(f, "{}", e),
        }
    }
}

impl From<mysql::Error> for ApiError {
    fn from(error: mysql::Error) -> Self {
        ApiError::DbError(error)
    }
}

impl From<bcrypt::BcryptError> for ApiError {
    fn from(error: bcrypt::BcryptError) -> Self {
        ApiError::BcryptError(error)
    }
}

impl From<ApiError> for Error {
    fn from(error: ApiError) -> Self {
        ErrorInternalServerError(error.to_string())
    }
}

#[derive(Debug, Serialize, Deserialize, FromRow)]
struct TaskStatus {
    id: i32,
    name: String,
}

#[derive(Debug, Serialize, Deserialize)]
struct Task {
    id: Option<i32>,
    title: String,
    description: Option<String>,
    status_id: i32,
    date_from: String,
    due_date: String,
}

#[derive(Debug, Serialize, Deserialize)]
struct TaskCreateRequest {
    title: String,
    description: Option<String>,
    status_id: i32,
    date_from: String,
    due_date: String,
}

fn is_valid_date(date_str: &str) -> bool {
    // Try different date formats
    NaiveDateTime::parse_from_str(date_str, "%Y-%m-%d %H:%M:%S").is_ok() ||
    NaiveDateTime::parse_from_str(date_str, "%d-%m-%Y %H:%M:%S").is_ok() ||
    NaiveDateTime::parse_from_str(&format!("{} 00:00:00", date_str), "%Y-%m-%d %H:%M:%S").is_ok() ||
    NaiveDateTime::parse_from_str(&format!("{} 00:00:00", date_str), "%d-%m-%Y %H:%M:%S").is_ok()
}

async fn create_task(task: web::Json<TaskCreateRequest>, pool: web::Data<Pool>) -> Result<HttpResponse, Error> {
    log::debug!("Received task creation request: {:?} to URL /api/tasks", task);
    
    // Validate title
    if task.title.trim().is_empty() {
        return Ok(HttpResponse::BadRequest().json(serde_json::json!({
            "success": false,
            "message": "El título es requerido"
        })));
    }
    
    if task.title.len() < 3 {
        return Ok(HttpResponse::BadRequest().json(serde_json::json!({
            "success": false,
            "message": "El título debe tener al menos 3 caracteres"
        })));
    }
    
    // Validate dates
    if !is_valid_date(&task.date_from) {
        return Ok(HttpResponse::BadRequest().json(serde_json::json!({
            "success": false,
            "message": "Formato de fecha inicial inválido. Formatos permitidos: YYYY-MM-DD, DD-MM-YYYY, o con hora (HH:MM:SS)",
            "example": {
                "title": "Ejemplo de tarea",
                "description": "Descripción de ejemplo",
                "status_id": 1,
                "date_from": "2024-01-21 00:00:00",
                "due_date": "2024-01-27 00:00:00"
            }
        })));
    }
    
    if !is_valid_date(&task.due_date) {
        return Ok(HttpResponse::BadRequest().json(serde_json::json!({
            "success": false,
            "message": "Formato de fecha final inválido. Formatos permitidos: YYYY-MM-DD, DD-MM-YYYY, o con hora (HH:MM:SS)",
            "example": {
                "title": "Ejemplo de tarea",
                "description": "Descripción de ejemplo",
                "status_id": 1,
                "date_from": "2024-01-21 00:00:00",
                "due_date": "2024-01-27 00:00:00"
            }
        })));
    }
    
    let mut conn = pool.get_conn()
        .map_err(|e| ApiError::DbError(e))?;
    
    // Validate status exists
    let status_exists: Option<i32> = conn.exec_first(
        "SELECT id FROM task_statuses WHERE id = ?",
        (task.status_id,)
    ).map_err(|e| ApiError::DbError(e))?;
    
    if status_exists.is_none() {
        return Ok(HttpResponse::BadRequest().json(serde_json::json!({
            "success": false,
            "message": "Estado de tarea inválido. Estados disponibles: 1 (Pendiente), 2 (Completado)",
            "example": {
                "title": "Ejemplo de tarea",
                "description": "Descripción de ejemplo",
                "status_id": 1,
                "date_from": "2024-01-21 00:00:00",
                "due_date": "2024-01-27 00:00:00"
            }
        })));
    }
    
    // Insert task
    let result = conn.exec_drop(
        "INSERT INTO tasks (title, description, status_id, date_from, due_date) VALUES (?, ?, ?, ?, ?)",
        (
            task.title.clone(),
            task.description.clone().unwrap_or_default(),
            task.status_id,
            task.date_from.clone(),
            task.due_date.clone()
        )
    ).map_err(|e| ApiError::DbError(e));
    
    match result {
        Ok(_) => Ok(HttpResponse::Ok().json(serde_json::json!({
            "success": true,
            "message": "Tarea creada exitosamente"
        }))),
        Err(e) => Ok(HttpResponse::InternalServerError().json(serde_json::json!({
            "success": false,
            "message": format!("Error al crear la tarea: {}", e)
        })))
    }
}

async fn get_task(id: web::Path<i32>, pool: web::Data<Pool>) -> Result<HttpResponse, Error> {
    let mut conn = pool.get_conn()
        .map_err(|e| ApiError::DbError(e))?;
    
    let task: Option<(i32, String, String, i32, String, String, String)> = conn
        .exec_first(
            "SELECT t.id, t.title, t.description, t.status_id, ts.name as status, t.date_from, t.due_date 
            FROM tasks t 
            JOIN task_statuses ts ON t.status_id = ts.id 
            WHERE t.id = ?",
            (id.into_inner(),)
        )
        .map_err(|e| ApiError::DbError(e))?;
    
    match task {
        Some((id, title, description, status_id, status, date_from, due_date)) => {
            Ok(HttpResponse::Ok().json(serde_json::json!({
                "success": true,
                "task": {
                    "id": id,
                    "title": title,
                    "description": description,
                    "status_id": status_id,
                    "status": status,
                    "date_from": date_from,
                    "due_date": due_date
                }
            })))
        }
        None => Ok(HttpResponse::NotFound().json(serde_json::json!({
            "success": false,
            "message": "Tarea no encontrada"
        })))
    }
}

async fn get_all_tasks(pool: web::Data<Pool>) -> Result<HttpResponse, Error> {
    let mut conn = pool.get_conn()
        .map_err(|e| ApiError::DbError(e))?;
    
    let tasks: Vec<(i32, String, String, i32, String, String, String)> = conn
        .exec(
            "SELECT t.id, t.title, t.description, t.status_id, ts.name as status, t.date_from, t.due_date 
            FROM tasks t 
            JOIN task_statuses ts ON t.status_id = ts.id 
            ORDER BY t.id",
            ()
        )
        .map_err(|e| ApiError::DbError(e))?;
    
    let task_list: Vec<serde_json::Value> = tasks.into_iter()
        .map(|(id, title, description, status_id, status, date_from, due_date)| {
            serde_json::json!({
                "id": id,
                "title": title,
                "description": description,
                "status_id": status_id,
                "status": status,
                "date_from": date_from,
                "due_date": due_date
            })
        })
        .collect();
    
    Ok(HttpResponse::Ok().json(serde_json::json!({
        "success": true,
        "tasks": task_list
    })))
}

async fn update_task(id: web::Path<i32>, task: web::Json<TaskCreateRequest>, pool: web::Data<Pool>) -> Result<HttpResponse, Error> {
    log::debug!("Received task update request for id {}: {:?}", id, task);
    
    // Validate title
    if task.title.trim().is_empty() {
        return Ok(HttpResponse::BadRequest().json(serde_json::json!({
            "success": false,
            "message": "El título es requerido"
        })));
    }
    
    if task.title.len() < 3 {
        return Ok(HttpResponse::BadRequest().json(serde_json::json!({
            "success": false,
            "message": "El título debe tener al menos 3 caracteres"
        })));
    }
    
    // Validate dates
    if !is_valid_date(&task.date_from) {
        return Ok(HttpResponse::BadRequest().json(serde_json::json!({
            "success": false,
            "message": "Formato de fecha inicial inválido. Use YYYY-MM-DD HH:MM:SS"
        })));
    }
    
    if !is_valid_date(&task.due_date) {
        return Ok(HttpResponse::BadRequest().json(serde_json::json!({
            "success": false,
            "message": "Formato de fecha final inválido. Use YYYY-MM-DD HH:MM:SS"
        })));
    }
    
    let mut conn = pool.get_conn()
        .map_err(|e| ApiError::DbError(e))?;
    
    // Validate status exists
    let status_exists: Option<i32> = conn.exec_first(
        "SELECT id FROM task_statuses WHERE id = ?",
        (task.status_id,)
    ).map_err(|e| ApiError::DbError(e))?;
    
    if status_exists.is_none() {
        return Ok(HttpResponse::BadRequest().json(serde_json::json!({
            "success": false,
            "message": "Estado de tarea inválido"
        })));
    }
    
    // Validate task exists
    let task_exists: Option<i32> = conn.exec_first(
        "SELECT id FROM tasks WHERE id = ?",
        (id.into_inner(),)
    ).map_err(|e| ApiError::DbError(e))?;
    
    if task_exists.is_none() {
        return Ok(HttpResponse::NotFound().json(serde_json::json!({
            "success": false,
            "message": "Tarea no encontrada"
        })));
    }
    
    // Update task
    let result = conn.exec_drop(
        "UPDATE tasks SET title = ?, description = ?, status_id = ?, date_from = ?, due_date = ? WHERE id = ?",
        (
            task.title.clone(),
            task.description.clone().unwrap_or_default(),
            task.status_id, 
            task.date_from.clone(),
            task.due_date.clone(),
            task_exists.unwrap()
        )
    ).map_err(|e| ApiError::DbError(e));
    
    match result {
        Ok(_) => Ok(HttpResponse::Ok().json(serde_json::json!({
            "success": true,
            "message": "Tarea actualizada exitosamente"
        }))),
        Err(e) => Ok(HttpResponse::InternalServerError().json(serde_json::json!({
            "success": false,
            "message": format!("Error al actualizar la tarea: {}", e)
        })))
    }
}

async fn delete_task(id: web::Path<i32>, pool: web::Data<Pool>) -> Result<HttpResponse, Error> {
    let mut conn = pool.get_conn()
        .map_err(|e| ApiError::DbError(e))?;
    
    // Validate task exists
    let task_exists: Option<i32> = conn.exec_first(
        "SELECT id FROM tasks WHERE id = ?",
        (id.into_inner(),)
    ).map_err(|e| ApiError::DbError(e))?;
    
    if task_exists.is_none() {
        return Ok(HttpResponse::NotFound().json(serde_json::json!({
            "success": false,
            "message": "Tarea no encontrada"
        })));
    }
    
    // Delete task
    let result = conn.exec_drop(
        "DELETE FROM tasks WHERE id = ?",
        (task_exists.unwrap(),)
    ).map_err(|e| ApiError::DbError(e));
    
    match result {
        Ok(_) => Ok(HttpResponse::Ok().json(serde_json::json!({
            "success": true,
            "message": "Tarea eliminada exitosamente"
        }))),
        Err(e) => Ok(HttpResponse::InternalServerError().json(serde_json::json!({
            "success": false,
            "message": format!("Error al eliminar la tarea: {}", e)
        })))
    }
}

#[derive(Debug, Serialize, Deserialize)]
struct TaskResponse {
    id: i32,
    title: String,
    description: String,
    status: String,
    date_from: String,
    due_date: String,
}

#[derive(Debug, Serialize, Deserialize)]
struct User {
    name: String,
    email: String, 
    password: String,
}

#[derive(Debug, Serialize, Deserialize)]
struct LoginRequest {
    email: String,
    password: String,
}
#[derive(Debug, Serialize, Deserialize)]
struct RegisterResponse {
    success: bool,
    message: String,
}

fn is_valid_email(email: &str) -> bool {
    email.contains('@') && email.contains('.')
}

#[derive(Debug, Serialize, Deserialize)]
struct UserData {
    name: String,
    email: String,
}

#[derive(Debug, Serialize, Deserialize)]
struct UserResponse {
    success: bool,
    message: String,
    user: UserData
}

#[derive(Debug, Serialize, Deserialize)]
struct LoginResponse {
    success: bool,
    message: String,
}

async fn login(credentials: web::Json<LoginRequest>, pool: web::Data<Pool>) -> Result<HttpResponse, Error> {
    log::debug!("Received login request for email: {}", credentials.email);
    let mut conn = pool.get_conn()
        .map_err(|e| ApiError::DbError(e))?;

    // Query for user with matching email
    let result = conn.exec_first::<(String, String, String), _, _>(
        "SELECT name, email, password FROM users WHERE email = ?",
        (credentials.email.clone(),),
    ).map_err(|e| ApiError::DbError(e))?;

    match result {
        Some((db_name, db_email, db_password)) => {
            // Verify password using bcrypt
            match verify(&credentials.password, &db_password).map_err(|e| ApiError::BcryptError(e))? {
                true => Ok(HttpResponse::Ok().json(UserResponse {
                    success: true,
                    message: "Inicio de sesión exitoso".to_string(),
                    user: UserData {
                        name: db_name,
                        email: db_email,
                    }
                })),
                false => Ok(HttpResponse::Unauthorized().json(LoginResponse {
                    success: false,
                    message: "Combinación de correo o contraseña inválida. Por favor intente de nuevo.".to_string(),
                })),
            }
        }
        None => Ok(HttpResponse::Unauthorized().json(LoginResponse {
            success: false,
            message: "No se encontró cuenta con este correo. Por favor regístrese primero.".to_string(),
        })),
    }
}

async fn register(user: web::Json<User>, pool: web::Data<Pool>) -> Result<HttpResponse, Error> {
    log::debug!("Received registration request: {:?}", user);
    
    // Validate name
    if user.name.trim().is_empty() {
        log::debug!("Validation failed: name is empty");
        return Ok(HttpResponse::BadRequest().json(RegisterResponse {
            success: false,
            message: "El nombre es requerido".to_string(),
        }));
    }
    
    if user.name.len() < 2 {
        log::debug!("Validation failed: name too short ({})", user.name.len());
        return Ok(HttpResponse::BadRequest().json(RegisterResponse {
            success: false,
            message: "El nombre debe tener al menos 2 caracteres".to_string(),
        }));
    }

    if !is_valid_email(&user.email) {
        log::debug!("Validation failed: invalid email format ({})", user.email);
        return Ok(HttpResponse::BadRequest().json(RegisterResponse {
            success: false,
            message: "Formato de correo inválido: debe contener @ y .".to_string(),
        }));
    }
        if user.password.len() < 6 {
            log::debug!("Validation failed: password too short ({})", user.password.len());
            return Ok(HttpResponse::BadRequest().json(RegisterResponse {
                success: false,
                message: "La contraseña debe tener al menos 6 caracteres por seguridad".to_string(),
            }));
        }

    let mut conn = pool.get_conn()
        .map_err(|e| ApiError::DbError(e))?;

    // Check if user already exists
    let existing_user: Option<String> = conn
        .exec_first(
            "SELECT email FROM users WHERE email = ?",
            (user.email.clone(),)
        )
        .map_err(|e| ApiError::DbError(e))?;

    if existing_user.is_some() {
        return Ok(HttpResponse::Conflict().json(RegisterResponse {
            success: false,
            message: "El usuario ya existe".to_string(),
        }));
    }

    // Hash password
    let hashed_password = bcrypt::hash(&user.password, DEFAULT_COST)
        .map_err(|e| ApiError::BcryptError(e))?;

    // Insert new user
    conn.exec_drop(
        "INSERT INTO users (name, email, password) VALUES (?, ?, ?)",
        (user.name.clone(), user.email.clone(), hashed_password),
    )
    .map_err(|e| ApiError::DbError(e))?;

    Ok(HttpResponse::Ok().json(RegisterResponse {
        success: true,
        message: "Usuario registrado exitosamente".to_string(),
    }))
}

#[actix_web::main]
async fn main() -> std::io::Result<()> {
    // Load environment variables from .env file
    dotenv::dotenv().ok();

    // Initialize logger
    env_logger::init_from_env(env_logger::Env::new().default_filter_or("info"));
    
    // Get database configuration from environment variables
    let db_user = env::var("DB_USER").expect("DB_USER must be set");
    let db_password = env::var("DB_PASSWORD").expect("DB_PASSWORD must be set");
    let db_host = env::var("DB_HOST").expect("DB_HOST must be set");
    let db_port = env::var("DB_PORT").expect("DB_PORT must be set");
    let db_name = env::var("DB_NAME").expect("DB_NAME must be set");
    
    // Create database connection string
    let database_url = format!(
        "mysql://{}:{}@{}:{}/{}",
        db_user, db_password, db_host, db_port, db_name
    );
    
    // Database connection setup
    let pool = Pool::new(database_url.as_str())
        .expect("Failed to create pool");

    log::info!("Server starting on http://localhost:8080");
    log::info!("Available endpoints:");
    log::info!("  POST /api/register - Register new user");
    log::info!("  POST /api/login - User login");
    log::info!("  GET /api/tasks - List all tasks");
    log::info!("  POST /api/tasks - Create new task");
    log::info!("  GET /api/tasks/{{id}} - Get task details");
    log::info!("  PUT /api/tasks/{{id}} - Update task");
    log::info!("  DELETE /api/tasks/{{id}} - Delete task");
    
    HttpServer::new(move || {
        // Log task routes registration
        log::debug!("Registering task routes:");
        log::debug!("  POST /api/tasks -> create_task");
        log::debug!("  GET /api/tasks -> get_all_tasks");
        log::debug!("  GET /api/tasks/{{id}} -> get_task");
        log::debug!("  PUT /api/tasks/{{id}} -> update_task");
        log::debug!("  DELETE /api/tasks/{{id}} -> delete_task");

        App::new()
            .wrap(Logger::default())
            .app_data(web::Data::new(pool.clone()))
            .app_data(
                web::JsonConfig::default()
                    .limit(4096)
                    .error_handler(|err, _| {
                        let error_msg = match &err {
                            JsonPayloadError::Serialize(e) => {
                                format!("Failed to serialize response: {}", e)
                            }
                            JsonPayloadError::Deserialize(e) => {
                                let err_string = e.to_string();
                                if err_string.contains("missing field") {
                                    let field = err_string
                                        .split("missing field `").nth(1)
                                        .and_then(|s| s.split('`').next())
                                        .unwrap_or("unknown");
                                    format!("Campo requerido faltante: {}", field)
                                } else {
                                    format!("Formato de datos inválido: {}", e)
                                }
                            }
                            JsonPayloadError::Payload(e) => {
                                format!("Error al leer los datos de la solicitud: {}", e)
                            }
                            JsonPayloadError::ContentType => {
                                "Tipo de contenido inválido".to_string()
                            }
                            JsonPayloadError::Overflow { .. } => {
                                "Datos de la solicitud muy grandes".to_string()
                            }
                            _ => "Formato JSON inválido".to_string()
                        };
                        
                        let response = HttpResponse::BadRequest()
                            .content_type("application/json")
                            .json(serde_json::json!({
                                "success": false,
                                "message": error_msg,
                            }));
                            
                        InternalError::from_response(err, response).into()
                    })
            )
            .service(web::resource("/").to(|| async {
                HttpResponse::Ok().json(serde_json::json!({
                    "name": "Task Management API",
                    "version": "1.0",
                    "endpoints": {
                        "auth": {
                            "register": "POST /api/register",
                            "login": "POST /api/login"
                        },
                        "tasks": {
                            "list": "GET /api/tasks",
                            "get": "GET /api/tasks/{id}",
                            "create": "POST /api/tasks",
                            "update": "PUT /api/tasks/{id}",
                            "delete": "DELETE /api/tasks/{id}"
                        }
                    }
                }))
            }))
            .service(
                web::scope("/api")
                    .service(web::resource("/login").route(web::post().to(login)))
                    .service(web::resource("/register").route(web::post().to(register)))
                    .service(
                        web::scope("/tasks")
                            .route("", web::post().to(create_task))
                            .route("", web::get().to(get_all_tasks))
                            .route("/{id}", web::get().to(get_task))
                            .route("/{id}", web::put().to(update_task))
                            .route("/{id}", web::delete().to(delete_task))
                    )
            )
    })
    .bind("127.0.0.1:8080")?
    .run()
    .await
}
