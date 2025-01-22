package com.example.taskappparalelos.repository;

import com.example.taskappparalelos.api.ITaskService;
import com.example.taskappparalelos.model.TaskResponse;
import com.example.taskappparalelos.network.RetrofitClientInstance;

import java.util.List;

import retrofit2.Call;
import retrofit2.Callback;
import retrofit2.Response;

public class TaskRepository {

    public interface ITaskResponse {
        void onResponse(List<TaskResponse.Task> tasks);
        void onFailure(Throwable t);
    }

    public void getTasks(ITaskResponse taskResponse) {
        ITaskService taskService = RetrofitClientInstance.getInstance().create(ITaskService.class);
        Call<TaskResponse> call = taskService.getTasks();

        call.enqueue(new Callback<TaskResponse>() {
            @Override
            public void onResponse(Call<TaskResponse> call, Response<TaskResponse> response) {
                if (response.isSuccessful() && response.body() != null && response.body().getTasks() != null) {
                    taskResponse.onResponse(response.body().getTasks());
                } else {
                    taskResponse.onFailure(new Throwable(response.message()));
                }
            }

            @Override
            public void onFailure(Call<TaskResponse> call, Throwable t) {
                taskResponse.onFailure(t);
            }
        });
    }
}
