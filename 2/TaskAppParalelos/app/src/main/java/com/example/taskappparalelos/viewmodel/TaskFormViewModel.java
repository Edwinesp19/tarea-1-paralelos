package com.example.taskappparalelos.viewmodel;

import android.view.View;

import androidx.lifecycle.LiveData;
import androidx.lifecycle.MutableLiveData;
import androidx.lifecycle.ViewModel;

import com.example.taskappparalelos.model.TaskBody;
import com.example.taskappparalelos.repository.TaskRepository;

public class TaskFormViewModel extends ViewModel {
    MutableLiveData<Integer> mProgressMutableData = new MutableLiveData<>();
    MutableLiveData<String> mTaskResultMutableData = new MutableLiveData<>();
    TaskRepository mTaskRepository;

    public TaskFormViewModel() {
        mProgressMutableData.postValue(View.INVISIBLE);
        mTaskResultMutableData.postValue("No action performed");
        mTaskRepository = new TaskRepository();
    }

    public void saveTask(TaskBody taskBody) {
        mProgressMutableData.postValue(View.VISIBLE);
        mTaskResultMutableData.postValue("Guardando...");
        mTaskRepository.saveTask(taskBody, new TaskRepository.ITaskFormResponse() {
            @Override
            public void onSuccess(String message) {
                mProgressMutableData.postValue(View.INVISIBLE);
                mTaskResultMutableData.postValue("Task saved successfully: " + message);
            }

            @Override
            public void onFailure(Throwable t) {
                mProgressMutableData.postValue(View.INVISIBLE);
                mTaskResultMutableData.postValue("Task saving failed: " + t.getLocalizedMessage());
            }
        });
    }

    public void updateTask(int taskId, TaskBody taskBody) {
        mProgressMutableData.postValue(View.VISIBLE);
        mTaskResultMutableData.postValue("Actualizando..");
        mTaskRepository.updateTask(taskId, taskBody, new TaskRepository.ITaskFormResponse() {
            @Override
            public void onSuccess(String message) {
                mProgressMutableData.postValue(View.INVISIBLE);
                mTaskResultMutableData.postValue(message);
            }

            @Override
            public void onFailure(Throwable t) {
                mProgressMutableData.postValue(View.INVISIBLE);
                mTaskResultMutableData.postValue("Task update failed: " + t.getLocalizedMessage());
            }
        });
    }

    public LiveData<Integer> getProgress() {
        return mProgressMutableData;
    }

    public LiveData<String> getTaskResult() {
        return mTaskResultMutableData;
    }
}
