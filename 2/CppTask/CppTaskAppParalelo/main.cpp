#include <iostream>
#include <fstream>
#include <sstream>
#include <thread>
#include <vector>
#include <string>
#include <ctime>
#include <chrono>
#include <mutex>
#include <filesystem> 



#include "sqlite/sqlite3.h"
std::mutex dbMutex;


void createDatabase(sqlite3* db);
void menu(sqlite3* db);
std::string createRandomTask(sqlite3* db);
std::string assignRandomTask(sqlite3* db);
void listTasks(sqlite3* db);
void insertTaskManually(sqlite3* db);
void importTasksFromCSV(sqlite3* db, const std::string& filename, int numTasks);

void parallelProcess(sqlite3* db,bool autoCreate);
void createRandomTaskProccess(sqlite3* db);
void assignTaskRandomUserProccess(sqlite3* db);
void saveTasksToFileProcess(sqlite3* db);

void simulateProcessing(int seconds);

int main() {
    sqlite3* db;
    if (sqlite3_open("rest_db.sqlite", &db) != SQLITE_OK) {
        std::cerr << "Error al abrir la base de datos: " << sqlite3_errmsg(db) << std::endl;
        return -1;
    }

    createDatabase(db);
    menu(db);
    sqlite3_close(db);
    return 0;
}

void menu(sqlite3* db) {
    int option;
    do {
        std::cout << "\n\n1. Crear tarea aleatoria\n";
        std::cout << "2. Asignar tarea a usuario\n";
        std::cout << "3. Listar tareas\n";
        std::cout << "4. Proceso en paralelo\n";
        std::cout << "5. Salir\n";
        std::cout << "Selecciona una opción: ";
        std::cin >> option;

        switch (option) {
        case 1:
            int choice;
            std::cout << "Selecciona el metodo de registro:\n";
            std::cout << "1. Manual\n";
            std::cout << "2. Automatico\n";
            std::cout << "Selecciona una opción: ";
            std::cin >> choice;

            if (choice == 1) {
                insertTaskManually(db);
            }
            else if (choice == 2) {
                std::cout << createRandomTask(db) << std::endl;
            } 

            break;
        case 2:
            std::cout << assignRandomTask(db) << std::endl;
            break;
        case 3:
            listTasks(db);
            break;
        case 4:
            int processChoice;
            std::cout << "Selecciona el método de proceso paralelo:\n";
            std::cout << "1. Manual\n";
            std::cout << "2. Automático\n";
            std::cout << "Selecciona una opción: ";
            std::cin >> processChoice;

            if (processChoice == 1) { 
                insertTaskManually(db);
            }
            else if (processChoice == 2) {
                parallelProcess(db,true);
            }
            break;
        case 5:
            std::cout << "Saliendo...\n";
            break;
        default:
            std::cout << "Opción no válida.\n";
        }
    } while (option != 5);
}

void parallelProcess(sqlite3* db,bool autoCreate) {
    using namespace std::chrono;

    auto start = high_resolution_clock::now();
    std::cout << "\033[33m\n\n-> INICIANDO PROCESOS EN HILOS...\n.\033[0m";
    if (autoCreate) {
        // Crear tarea en un hilo separado

        std::thread createThread(createRandomTaskProccess, db);
        createThread.join();
    } 

    // Asignar una tarea a un  usuario en otro hilo separado
    std::thread assignThread(assignTaskRandomUserProccess, db );

    // Guardar lista de tareas en archivo (otro hilo)
    std::thread saveFileThread(saveTasksToFileProcess, db);
  
    // Esperar a que ambos hilos terminen 
    assignThread.join();
    saveFileThread.join();
   
    auto end = high_resolution_clock::now();
    auto duration = duration_cast<milliseconds>(end - start);

    std::cout << "\033[1;32m\n-> PROCESOS DE HILOS COMPLETADOS EN " << duration.count() << " ms.\n\n .\033[0m";
}

void simulateProcessing(int seconds) {
    std::this_thread::sleep_for(std::chrono::seconds(seconds));
}

void createRandomTaskProccess(sqlite3* db) {
    using namespace std::chrono;
    auto start = high_resolution_clock::now();

    std::cout << "\n> Iniciando creación de tarea...\n";

    std::string result = createRandomTask(db);
    std::cout << result << std::endl;

    auto end = high_resolution_clock::now();
    auto duration = duration_cast<milliseconds>(end - start);

    std::cout << "\n-> Proceso creación de tarea completado en " << duration.count() << " ms.\n";
}

void assignTaskRandomUserProccess(sqlite3* db) {
    using namespace std::chrono;
    auto start = high_resolution_clock::now();

    std::cout << "\n> Iniciando asignación de tarea a usuario...\n";

    std::string result = assignRandomTask(db);
    std::cout << result << std::endl;

    auto end = high_resolution_clock::now();
    auto duration = duration_cast<milliseconds>(end - start);

    std::cout << "\n -> Proceso de asignación de usuario completado en " << duration.count() << " ms.\n";
}

// Guardar tareas en archivo (proceso)
void saveTasksToFileProcess(sqlite3* db) {
    std::lock_guard<std::mutex> lock(dbMutex);
        using namespace std::chrono;

    auto start = high_resolution_clock::now();
    const char* selectSQL = "SELECT id, title, description FROM tasks;";
    sqlite3_stmt* stmt;

    std::ofstream outFile("tasks_list.txt");
    if (!outFile.is_open()) {
        std::cerr << "Error al abrir el archivo para escritura.\n";
        return;
    }

    sqlite3_prepare_v2(db, selectSQL, -1, &stmt, nullptr);
    while (sqlite3_step(stmt) == SQLITE_ROW) {
        int id = sqlite3_column_int(stmt, 0);
        const char* title = reinterpret_cast<const char*>(sqlite3_column_text(stmt, 1));
        const char* description = reinterpret_cast<const char*>(sqlite3_column_text(stmt, 2));
        outFile << "ID: " << id << ", Título: " << title << ", Descripción: " << description << "\n";
    }
    sqlite3_finalize(stmt);

    outFile.close();
    auto end = high_resolution_clock::now();
    auto duration = duration_cast<milliseconds>(end - start);
     std::cout << "\n-> Tareas guardadas en archivo en " << duration.count() << " ms.\n";
}

void listTasks(sqlite3* db) {
    const char* selectSQL = "SELECT id, title, description FROM tasks;";
    sqlite3_stmt* stmt;

    sqlite3_prepare_v2(db, selectSQL, -1, &stmt, nullptr);
    std::cout << "\nLista de tareas registradas:\n";
    while (sqlite3_step(stmt) == SQLITE_ROW) {
        int id = sqlite3_column_int(stmt, 0);
        const char* title = reinterpret_cast<const char*>(sqlite3_column_text(stmt, 1));
        const char* description = reinterpret_cast<const char*>(sqlite3_column_text(stmt, 2));

        std::cout << "ID: " << id << ", Título: " << title
            << ", Descripción: " << description << "\n";
    }

    sqlite3_finalize(stmt);
}

std::string createRandomTask(sqlite3* db) {
    std::vector<std::string> titles = {
        "Tarea 1", "Tarea 2", "Tarea 3", "Tarea 4" };
    std::vector<std::string> descriptions = { "Descripción 1", "Descripción 2", "Descripción 3", "Descripción 4" };

    srand(time(nullptr));
    std::string title = titles[rand() % titles.size()];
    std::string description = descriptions[rand() % descriptions.size()];

    const char* insertSQL = R"(
        INSERT INTO tasks (title, description, status_id, date_from, due_date)
        VALUES (?, ?, 1, date('now'), date('now', '+7 days'));
    )";

    sqlite3_stmt* stmt;
    sqlite3_prepare_v2(db, insertSQL, -1, &stmt, nullptr);
    sqlite3_bind_text(stmt, 1, title.c_str(), -1, SQLITE_STATIC);
    sqlite3_bind_text(stmt, 2, description.c_str(), -1, SQLITE_STATIC);

    std::string result;
    if (sqlite3_step(stmt) == SQLITE_DONE) {
        int taskId = sqlite3_last_insert_rowid(db);
        result = "Tarea creada exitosamente con ID " + std::to_string(taskId) +
            " y título: " + title + ".";
    }
    else {
        result = "Error al crear tarea: " + std::string(sqlite3_errmsg(db));
    }

    sqlite3_finalize(stmt); 
    return result;
}

std::string assignRandomTask(sqlite3* db) {
    const char* selectTaskSQL = "SELECT id FROM tasks ORDER BY RANDOM() LIMIT 1;";
    const char* selectUserSQL = "SELECT id FROM users ORDER BY RANDOM() LIMIT 1;";
    const char* insertAssignmentSQL = R"(
        INSERT INTO task_assignment (task_id, user_id) VALUES (?, ?);
    )";

    int taskId = -1, userId = -1;
    sqlite3_stmt* stmt;

    sqlite3_prepare_v2(db, selectTaskSQL, -1, &stmt, nullptr);
    if (sqlite3_step(stmt) == SQLITE_ROW) {
        taskId = sqlite3_column_int(stmt, 0);
    }
    sqlite3_finalize(stmt);

    sqlite3_prepare_v2(db, selectUserSQL, -1, &stmt, nullptr);
    if (sqlite3_step(stmt) == SQLITE_ROW) {
        userId = sqlite3_column_int(stmt, 0);
    }
    sqlite3_finalize(stmt);

    std::string result;
    if (taskId != -1 && userId != -1) {
        sqlite3_prepare_v2(db, insertAssignmentSQL, -1, &stmt, nullptr);
        sqlite3_bind_int(stmt, 1, taskId);
        sqlite3_bind_int(stmt, 2, userId);

        if (sqlite3_step(stmt) == SQLITE_DONE) {
            result = "\nTarea con ID " + std::to_string(taskId) +
                " asignada correctamente al usuario con ID " +
                std::to_string(userId) + ".\n";
        }
        else {
            result = "Error al asignar tarea: " + std::string(sqlite3_errmsg(db));
        }
        sqlite3_finalize(stmt);
    }
    else {
        result = "\nNo se encontraron tareas o usuarios para asignar.\n";
    }

    return result;
}


void insertTaskManually(sqlite3* db) {
    std::string title, description;
    std::cout << "Introduce el título de la tarea: ";
    std::cin.ignore(); 
    std::getline(std::cin, title);

    std::cout << "Introduce la descripción de la tarea: ";
    std::getline(std::cin, description);

    const char* insertSQL = R"(
        INSERT INTO tasks (title, description, status_id, date_from, due_date)
        VALUES (?, ?, 1, date('now'), date('now', '+7 days'));
    )";

    sqlite3_stmt* stmt;
    sqlite3_prepare_v2(db, insertSQL, -1, &stmt, nullptr);
    sqlite3_bind_text(stmt, 1, title.c_str(), -1, SQLITE_STATIC);
    sqlite3_bind_text(stmt, 2, description.c_str(), -1, SQLITE_STATIC);

    if (sqlite3_step(stmt) == SQLITE_DONE) {
        std::cout << "Tarea agregada exitosamente." << std::endl;
    }
    else {
        std::cerr << "Error al agregar tarea: " << sqlite3_errmsg(db) << std::endl;
    }

    sqlite3_finalize(stmt);
    parallelProcess(db,false);
}


void importTasksFromCSV(sqlite3* db, const std::string& filename, int numTasks) {
    std::ifstream file(filename);
    if (!file.is_open()) {
        std::cerr << "No se pudo abrir el archivo CSV.\n";
        return;
    }

    std::string line;
    int lineCount = 0;

    while (std::getline(file, line) && lineCount < numTasks) {
        std::stringstream ss(line);
        std::string title, description;

        std::getline(ss, title, ',');
        std::getline(ss, description);

        const char* insertSQL = R"(
            INSERT INTO tasks (title, description, status_id, date_from, due_date)
            VALUES (?, ?, 1, date('now'), date('now', '+7 days'));
        )";

        sqlite3_stmt* stmt;
        sqlite3_prepare_v2(db, insertSQL, -1, &stmt, nullptr);
        sqlite3_bind_text(stmt, 1, title.c_str(), -1, SQLITE_STATIC);
        sqlite3_bind_text(stmt, 2, description.c_str(), -1, SQLITE_STATIC);

        if (sqlite3_step(stmt) != SQLITE_DONE) {
            std::cerr << "Error al insertar tarea desde CSV: " << sqlite3_errmsg(db) << std::endl;
        }

        sqlite3_finalize(stmt);
        lineCount++;
    }

    file.close();
    std::cout << "Importación de tareas desde CSV completada.\n";
}

void createDatabase(sqlite3* db) {
    const char* schema = R"(
        CREATE TABLE IF NOT EXISTS users (
            id INTEGER PRIMARY KEY AUTOINCREMENT,
            name TEXT NOT NULL,
            email TEXT NOT NULL UNIQUE,
            password TEXT NOT NULL,
            created_at DATETIME DEFAULT CURRENT_TIMESTAMP,
            updated_at DATETIME DEFAULT CURRENT_TIMESTAMP
        );

        CREATE TABLE IF NOT EXISTS task_statuses (
            id INTEGER PRIMARY KEY AUTOINCREMENT,
            name TEXT NOT NULL UNIQUE
        );

        INSERT OR IGNORE INTO task_statuses (id, name) VALUES 
            (1, 'Pendiente'),
            (2, 'Completado');

        CREATE TABLE IF NOT EXISTS tasks (
            id INTEGER PRIMARY KEY AUTOINCREMENT,
            title TEXT NOT NULL,
            description TEXT,
            status_id INTEGER NOT NULL,
            date_from DATE NOT NULL,
            due_date DATE NOT NULL,
            created_at DATETIME DEFAULT CURRENT_TIMESTAMP,
            updated_at DATETIME DEFAULT CURRENT_TIMESTAMP,
            FOREIGN KEY (status_id) REFERENCES task_statuses(id)
        );

        CREATE TABLE IF NOT EXISTS task_assignment (
            id INTEGER PRIMARY KEY AUTOINCREMENT,
            task_id INTEGER NOT NULL,
            user_id INTEGER NOT NULL,
            assigned_at DATETIME DEFAULT CURRENT_TIMESTAMP,
            FOREIGN KEY (task_id) REFERENCES tasks(id),
            FOREIGN KEY (user_id) REFERENCES users(id)
        );
    )";

    char* errorMessage = nullptr;
    if (sqlite3_exec(db, schema, nullptr, nullptr, &errorMessage) != SQLITE_OK) {
        std::cerr << "Error al crear la base de datos: " << errorMessage << std::endl;
        sqlite3_free(errorMessage);
    }
    else {
        std::cout << "Base de datos y tablas creadas correctamente." << std::endl;
    }
}