package com.example.taskappparalelos.view;

import android.content.Intent;
import android.net.Uri;
import android.os.Bundle;
import android.view.View;
import android.widget.Button;
import android.widget.GridView;
import android.widget.ProgressBar;
import android.widget.Toast;

import androidx.activity.result.ActivityResultLauncher;
import androidx.activity.result.contract.ActivityResultContracts;
import androidx.appcompat.app.AppCompatActivity;
import androidx.lifecycle.ViewModelProvider;

import com.example.taskappparalelos.R;
import com.example.taskappparalelos.utils.FileUtils;
import com.example.taskappparalelos.viewmodel.ImageViewModel;
import com.example.taskappparalelos.adapters.ImagePreviewAdapter;

import java.util.ArrayList;
import java.util.List;

public class ImageActivity extends AppCompatActivity {
    private ImageViewModel imageViewModel;
    private GridView gridView;
    private ProgressBar progressBar;
    private Button btnUpload;

    private final List<String> selectedImagePaths = new ArrayList<>();
    private final List<Uri> selectedImageUris = new ArrayList<>();

    private final ActivityResultLauncher<String> imagePickerLauncher = registerForActivityResult(
            new ActivityResultContracts.GetMultipleContents(),
            uris -> {
                selectedImageUris.clear();
                selectedImagePaths.clear();

                for (Uri uri : uris) {
                    selectedImageUris.add(uri);
                    selectedImagePaths.add(FileUtils.getPathFromUri(this, uri)); // Utiliza una función para obtener la ruta real del archivo
                }

                gridView.setAdapter(new ImagePreviewAdapter(this, selectedImageUris)); // Configura un adaptador para mostrar las imágenes
            }
    );

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_image);

        imageViewModel = new ViewModelProvider(this).get(ImageViewModel.class);

        gridView = findViewById(R.id.gridView);
        progressBar = findViewById(R.id.progressBar);
        btnUpload = findViewById(R.id.btnUpload);

        findViewById(R.id.btnSelectImages).setOnClickListener(v -> imagePickerLauncher.launch("image/*"));

        btnUpload.setOnClickListener(v -> {
            if (selectedImagePaths.isEmpty()) {
                Toast.makeText(this, "Selecciona al menos una imagen", Toast.LENGTH_SHORT).show();
            } else {
                progressBar.setVisibility(View.VISIBLE);
                imageViewModel.uploadImages(selectedImagePaths);
            }
        });

        imageViewModel.getUploadStatus().observe(this, status -> {
            progressBar.setVisibility(View.GONE);
            Toast.makeText(this, status, Toast.LENGTH_SHORT).show();
        });
    }
}
