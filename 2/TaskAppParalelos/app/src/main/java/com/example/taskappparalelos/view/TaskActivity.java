package com.example.taskappparalelos.view;

import android.content.Intent;
import android.os.Bundle;
import android.os.Parcelable;
import android.view.LayoutInflater;
import android.view.View;
import android.widget.Button;
import android.widget.LinearLayout;
import android.widget.ProgressBar;
import android.widget.TextView;
import android.widget.Toast;

import androidx.appcompat.app.AppCompatActivity;
import androidx.appcompat.widget.ButtonBarLayout;
import androidx.lifecycle.Observer;
import androidx.lifecycle.ViewModelProvider;

import com.example.taskappparalelos.R;
import com.example.taskappparalelos.model.Task;
import com.example.taskappparalelos.model.TaskResponse;
import com.example.taskappparalelos.viewmodel.TaskViewModel;


import java.util.List;

public class TaskActivity extends AppCompatActivity {
    private TaskViewModel mViewModel;
    private LinearLayout taskContainer;
    private ProgressBar progressBar;



    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_task);

        // Recuperar datos del intent (si se enviaron)
        String username = getIntent().getStringExtra("username");

// Mostrar los datos (opcional)
        TextView textView = findViewById(R.id.textViewWelcome);
        textView.setText("Welcome, " + username + "!");


        taskContainer = findViewById(R.id.taskContainer);
        progressBar = findViewById(R.id.progressBar);

        mViewModel = new ViewModelProvider(this).get(TaskViewModel.class);

        mViewModel.getProgress().observe(this, new Observer<Integer>() {
            @Override
            public void onChanged(Integer visibility) {
                progressBar.setVisibility(visibility);
            }
        });

        mViewModel.getTasks().observe(this, new Observer<List<TaskResponse.Task>>() {
            @Override
            public void onChanged(List<TaskResponse.Task> tasks) {
                displayTasks(tasks);
            }
        });



        // Fetch tasks on Activity start
        mViewModel.fetchTasks();

    }

    @Override
    protected void onActivityResult(int requestCode, int resultCode, Intent data) {
        super.onActivityResult(requestCode, resultCode, data);

        if (requestCode == 1 && resultCode == RESULT_OK) {
            // Recargar la lista de tareas
            mViewModel.fetchTasks();
        }
    }



    private void displayTasks(List<TaskResponse.Task> tasks) {
        taskContainer.removeAllViews(); // Limpia las vistas anteriores
        if (tasks != null && !tasks.isEmpty()) {
            // Mostrar tareas

        for (TaskResponse.Task task : tasks) {
            // Infla un layout personalizado para cada tarea
            View taskView = LayoutInflater.from(this).inflate(R.layout.item_task, taskContainer, false);

            // Asigna los datos de la tarea a los elementos de la vista
            TextView tvTitle = taskView.findViewById(R.id.tvTaskTitle);
            TextView tvDescription = taskView.findViewById(R.id.tvTaskDescription);
            TextView tvDueDate = taskView.findViewById(R.id.tvTaskDueDate);
            TextView tvStatus = taskView.findViewById(R.id.tvTaskStatus);

            tvTitle.setText(task.getTitle());
            tvDescription.setText(task.getDescription());
            tvDueDate.setText("Due: " + task.getDueDate());
            tvStatus.setText(task.getStatus());

            Task taskData = new Task(
                    task.getId(),
                    task.getTitle(),
                    task.getDescription(),
                    task.getDateFrom(),
                    task.getDueDate(),
                    task.getStatus(),
                    task.getStatusId()
            );

            //navegar a la pantalla de fomulario
            taskView.setOnClickListener(new View.OnClickListener() {
                @Override
                public void onClick(View v) {
                    Intent intent = new Intent(TaskActivity.this, TaskFormActivity.class);
                    intent.putExtra("task", taskData);
                    startActivityForResult(intent, 1); // Código de solicitud 1
                }
            });


            // Añade la vista al contenedor
            taskContainer.addView(taskView);
        }} else {
            Toast.makeText(this, "No tasks available", Toast.LENGTH_SHORT).show();
        }
    }
}
