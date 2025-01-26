package com.example.taskappparalelos.viewmodel;

import androidx.lifecycle.LiveData;
import androidx.lifecycle.MutableLiveData;
import androidx.lifecycle.ViewModel;

import com.example.taskappparalelos.repository.ImageRepository;

import java.util.List;

public class ImageViewModel extends ViewModel {
    private final ImageRepository imageRepository;
    private final MutableLiveData<String> uploadStatus = new MutableLiveData<>();

    public ImageViewModel() {
        imageRepository = new ImageRepository();
    }

    public LiveData<String> getUploadStatus() {
        return uploadStatus;
    }

    public void uploadImages(List<String> imagePaths) {
        imageRepository.uploadImages(imagePaths, new ImageRepository.ImageUploadCallback() {
            @Override
            public void onSuccess(String message) {
                uploadStatus.setValue(message);
            }

            @Override
            public void onFailure(Throwable t) {
                uploadStatus.setValue(t.getMessage());
            }
        });
    }
}
