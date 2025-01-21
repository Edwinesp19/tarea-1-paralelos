import { TaskModel } from '../models/Task';
import { Http } from "@nativescript/core";

export default class TaskService {
  private tasks: TaskModel[] = [];

  async getTasks(): Promise<TaskModel[]> {
    try {
      const result: any = await Http.getJSON('http://127.0.0.1:8000/api/tasks');
      console.log(result);

      const tasks: TaskModel[] = result.data.map((task: any) => ({
        id: task.id,
        title: task.title,
        description: task.description,
        date_from: task.date_from,
        due_date: task.due_date,
        status: {
          id: task.status.id,
          name: task.status.name,
        },
      }));

      return tasks;
    } catch (error) {
      console.error('Error fetching tasks:', error);
      return [];
    }
  }

  async createTask(task: {
    title: string;
    description: string;
    status_id: number;
    date_from: string;
    due_date: string;
  }): Promise<TaskModel | null> {
    try {
      const response: any = await Http.request({
        url: 'http://127.0.0.1:8000/api/tasks',
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        content: JSON.stringify(task),
      });

      console.log('Task created:', response);
      return response.data as TaskModel;
    } catch (error) {
      console.error('Error creating task:', error);
      return null;
    }
  }

  getTaskById(id: number): TaskModel | undefined {
    return this.tasks.find((task) => task.id === id) || undefined;
  }
}
