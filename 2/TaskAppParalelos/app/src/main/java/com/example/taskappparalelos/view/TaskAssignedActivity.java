package com.example.taskappparalelos.view;

import androidx.appcompat.app.AppCompatActivity;

import android.content.Intent;
import android.os.Bundle;
import android.view.View;
import android.widget.ImageView;

import com.example.taskappparalelos.R;
import com.google.android.material.bottomnavigation.BottomNavigationView;

public class TaskAssignedActivity extends AppCompatActivity {
    ImageView btnBack;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_task_assigned);
        btnBack = findViewById(R.id.btnBack);
        btnBack.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                navBack();
            }
        });
    }

    private void navBack() {
        setResult(RESULT_OK); // Indica que hubo un cambio en los datos
        finish(); // Cierra la actividad actual
    }
}