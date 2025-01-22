package com.example.taskappparalelos.view;

import androidx.appcompat.app.AppCompatActivity;
import android.os.Bundle;
import android.widget.TextView;

import com.example.taskappparalelos.R;

public class TaskActivity extends AppCompatActivity {

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_task);

        // Recuperar datos del intent (si se enviaron)
        String username = getIntent().getStringExtra("username");

        // Mostrar los datos (opcional)
        TextView textView = findViewById(R.id.textViewWelcome);
        textView.setText("Welcome, " + username + "!");
    }
}