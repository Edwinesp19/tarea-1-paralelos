<?php

namespace App\Http\Controllers;

use Illuminate\Http\Request;
use App\Models\Task;

class TaskController extends Controller
{
    public function index()
    {
        $tasks = Task::with('status')->get();

        return response()->json([
            'status' => 'success',
            'data' => $tasks
        ], 200);


    }
    public function store(Request $request)
    {
        $this->validate($request, [
            'title' => 'required|string|max:255',
            'description' => 'nullable|string',
            'status_id' => 'required|integer|exists:statuses,id',
            'date_from' => 'required|date',
            'due_date' => 'required|date|after_or_equal:date_from'
        ]);

        $task = Task::create([
            'title' => $request->input('title'),
            'description' => $request->input('description'),
            'status_id' => $request->input('status_id'),
            'date_from' => $request->input('date_from'),
            'due_date' => $request->input('due_date')
        ]);

        return response()->json([
            'status' => 'success',
            'data' => $task
        ], 201);
    }
}
