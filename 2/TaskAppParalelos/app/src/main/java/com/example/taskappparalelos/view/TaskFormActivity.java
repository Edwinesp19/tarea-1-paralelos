package com.example.taskappparalelos.view;

import androidx.appcompat.app.AppCompatActivity;

import android.content.Intent;
import android.os.Bundle;
import android.widget.Button;
import android.widget.EditText;
import android.widget.TextView;
import android.app.DatePickerDialog;
import android.widget.Toast;
import java.util.Calendar;
import android.widget.Spinner;
import android.widget.ProgressBar;
import androidx.lifecycle.Observer;
import androidx.lifecycle.ViewModelProvider;




import com.example.taskappparalelos.R;
import com.example.taskappparalelos.model.Task;
import com.example.taskappparalelos.model.TaskBody;
import com.example.taskappparalelos.model.TaskResponse;
import com.example.taskappparalelos.viewmodel.TaskFormViewModel;

public class TaskFormActivity extends AppCompatActivity {

    EditText etTaskTitle;
    EditText etTaskDescription;
    EditText etTaskDateFrom;
    EditText etTaskDueDate;
    ProgressBar progressBar;

    TextView tvTaskFormTitle;
    Spinner spStatusId;

    TaskFormViewModel mViewModel;

    int taskIdToUpdate = 1;


    Button btnSave,bUpdateTask;
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
            btnSave = findViewById(R.id.btnSave);
            spStatusId = findViewById(R.id.spStatusId);
            progressBar = findViewById(R.id.progressBar);
            bUpdateTask = findViewById(R.id.bUpdateTask);
            tvTaskFormTitle= findViewById(R.id.tvTaskFormTitle);
            taskIdToUpdate = task.getId();



            mViewModel = new ViewModelProvider(this).get(TaskFormViewModel.class);

            // Establece los valores de los campos de entrada
            etTaskTitle.setText(task.getTitle());
            etTaskDescription.setText(task.getDescription());
            etTaskDateFrom.setText(task.getDateFrom());
            etTaskDueDate.setText(task.getDueDate());
            tvTaskFormTitle.setText("Tarea #"+task.getId());


            // Abrir DatePicker para "Date From"
            etTaskDateFrom.setOnClickListener(v -> showDatePickerDialog(etTaskDateFrom));

            // Abrir DatePicker para "Due Date"
            etTaskDueDate.setOnClickListener(v -> showDatePickerDialog(etTaskDueDate));

            // Acción del botón de guardar
            btnSave.setOnClickListener(v -> saveTask());

            bUpdateTask.setOnClickListener(v -> {
                String title = etTaskTitle.getText().toString();
                String description = etTaskDescription.getText().toString();
                String dateFrom = etTaskDateFrom.getText().toString();
                String dueDate = etTaskDueDate.getText().toString();
//                int statusId = spStatusId.getSelectedItemPosition() + 1;
                int statusId =  1;
                TaskBody taskBody = new TaskBody(title, description, dateFrom, dueDate, statusId);
                mViewModel.updateTask(taskIdToUpdate, taskBody);
            });

            mViewModel.getTaskResult().observe(this, new Observer<String>() {
                @Override
                public void onChanged(String s) {
                    showMessage(s);
//                    Intent intent = new Intent(TaskFormActivity.this, TaskActivity.class);
//                    intent.addFlags(Intent.FLAG_ACTIVITY_CLEAR_TOP | Intent.FLAG_ACTIVITY_SINGLE_TOP);
//                    intent.putExtra("refresh", true);
//                    startActivity(intent);
//                    finish(); // Finalizar TaskFormActivity
                }
            });
        }

    }

    private void showDatePickerDialog(EditText editText) {
        Calendar calendar = Calendar.getInstance();
        int year = calendar.get(Calendar.YEAR);
        int month = calendar.get(Calendar.MONTH);
        int day = calendar.get(Calendar.DAY_OF_MONTH);

        DatePickerDialog datePickerDialog = new DatePickerDialog(this, (view, year1, month1, dayOfMonth) -> {
            // Formatear la fecha seleccionada y ponerla en el EditText
            String date = dayOfMonth + "-" + (month1 + 1) + "-" + year1;
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
        int statusId = 1;

        if (title.isEmpty() || description.isEmpty() || dateFrom.isEmpty() || dueDate.isEmpty()) {
            Toast.makeText(this, "Llena los campos", Toast.LENGTH_SHORT).show();
        } else {
//            Toast.makeText(this, "Tarea guardada!", Toast.LENGTH_SHORT).show();
            TaskBody taskBody = new TaskBody(title, description, dateFrom, dueDate, statusId);
            mViewModel.saveTask(taskBody);
        }
    }

    private void showMessage(String message) {
            Toast.makeText(this, message, Toast.LENGTH_SHORT).show();
    }

}