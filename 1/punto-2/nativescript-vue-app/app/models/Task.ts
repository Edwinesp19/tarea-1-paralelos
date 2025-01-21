// app/models/Task.ts

export interface TaskModel {
  id: number
  title: string
  description: string
  date_from: string
  due_date: string
  status: {
    id: number
    name: string
  }|null
}
