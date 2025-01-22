package com.example.taskappparalelos.api;

import com.example.taskappparalelos.model.TaskResponse;

import retrofit2.Call;
import retrofit2.http.GET;

public interface ITaskService {
    @GET("/api/tasks")
    Call<TaskResponse> getTasks();
}