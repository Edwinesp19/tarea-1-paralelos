package com.example.taskappparalelos.repository;

import com.example.taskappparalelos.api.IImageService;
import com.example.taskappparalelos.network.RetrofitClientInstance;

import java.util.List;

import okhttp3.MediaType;
import okhttp3.MultipartBody;
import okhttp3.RequestBody;
import okhttp3.ResponseBody;
import retrofit2.Call;
import retrofit2.Callback;
import retrofit2.Response;

public class ImageRepository {

    private static final String UNSPLASH_ACCESS_KEY = "pKAOSFoV47l0pWRbsIml_0MOcCiejBGgoHBipfU2pk4";
    private static final String UNSPLASH_API_URL = "https://api.unsplash.com/search/photos?page=1&limit=50&query=%s&client_id="+UNSPLASH_ACCESS_KEY;

    public void uploadImages(List<String> imagePaths, ImageUploadCallback callback) {
        IImageService imageService = RetrofitClientInstance.getInstance().create(IImageService.class);
        MultipartBody.Part[] imageParts = new MultipartBody.Part[imagePaths.size()];

        for (int i = 0; i < imagePaths.size(); i++) {
            RequestBody requestBody = RequestBody.create(MediaType.parse("image/*"), new java.io.File(imagePaths.get(i)));
            imageParts[i] = MultipartBody.Part.createFormData("images", new java.io.File(imagePaths.get(i)).getName(), requestBody);
        }

        Call<ResponseBody> call = imageService.uploadImages(imageParts);
        call.enqueue(new Callback<ResponseBody>() {
            @Override
            public void onResponse(Call<ResponseBody> call, Response<ResponseBody> response) {
                if (response.isSuccessful()) {
                    callback.onSuccess("Imágenes subidas exitosamente");
                } else {
                    callback.onFailure(new Throwable("Error en la subida"));
                }
            }

            @Override
            public void onFailure(Call<ResponseBody> call, Throwable t) {
                callback.onFailure(t);
            }
        });
    }

    public interface ImageUploadCallback {
        void onSuccess(String message);
        void onFailure(Throwable t);
    }

}
