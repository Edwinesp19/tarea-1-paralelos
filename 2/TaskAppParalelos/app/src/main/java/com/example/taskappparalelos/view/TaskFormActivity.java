package com.example.taskappparalelos.view;

import androidx.appcompat.app.AppCompatActivity;

import android.os.Bundle;
import android.widget.Button;
import android.widget.EditText;
import android.widget.TextView;
import android.app.DatePickerDialog;
import android.widget.Toast;
import java.util.Calendar;


import com.example.taskappparalelos.R;
import com.example.taskappparalelos.model.Task;
import com.example.taskappparalelos.model.TaskResponse;

public class TaskFormActivity extends AppCompatActivity {

    EditText etTaskTitle;
    EditText etTaskDescription;
    EditText etTaskDateFrom;
    EditText etTaskDueDate;

    Button btnSave;
    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_task_form);

        Task task = getIntent().getParcelableExtra("task");

        if (task != null) {
            // Usa los datos del objeto Task
            etTaskTitle = findViewById(R.id.etTaskTitle);
            etTaskDescription = findViewById(R.id.etTaskDescription);
            etTaskDateFrom = findViewById(R.id.etTaskDateFrom);
            etTaskDueDate = findViewById(R.id.etTaskDueDate);
            btnSave = findViewById(R.id.btnSave); // Este es el botón con el problema



            // Establece los valores de los campos de entrada
            etTaskTitle.setText(task.getTitle());
            etTaskDescription.setText(task.getDescription());
            etTaskDateFrom.setText(task.getDateFrom());
            etTaskDueDate.setText(task.getDueDate());


            // Abrir DatePicker para "Date From"
            etTaskDateFrom.setOnClickListener(v -> showDatePickerDialog(etTaskDateFrom));

            // Abrir DatePicker para "Due Date"
            etTaskDueDate.setOnClickListener(v -> showDatePickerDialog(etTaskDueDate));

            // Acción del botón de guardar
            btnSave.setOnClickListener(v -> saveTask());
        }
    }

    private void showDatePickerDialog(EditText editText) {
        Calendar calendar = Calendar.getInstance();
        int year = calendar.get(Calendar.YEAR);
        int month = calendar.get(Calendar.MONTH);
        int day = calendar.get(Calendar.DAY_OF_MONTH);

        DatePickerDialog datePickerDialog = new DatePickerDialog(this, (view, year1, month1, dayOfMonth) -> {
            // Formatear la fecha seleccionada y ponerla en el EditText
            String date = dayOfMonth + "/" + (month1 + 1) + "/" + year1;
            editText.setText(date);
        }, year, month, day);

        datePickerDialog.show();
    }

    private void saveTask() {
        // Guardar la tarea (puedes agregar la lógica para persistirla)
        String title = etTaskTitle.getText().toString();
        String description = etTaskDescription.getText().toString();
        String dateFrom = etTaskDateFrom.getText().toString();
        String dueDate = etTaskDueDate.getText().toString();

        if (title.isEmpty() || description.isEmpty() || dateFrom.isEmpty() || dueDate.isEmpty()) {
            Toast.makeText(this, "Llena los campos", Toast.LENGTH_SHORT).show();
        } else {
            Toast.makeText(this, "Tarea guardada!", Toast.LENGTH_SHORT).show();
        }
    }
}