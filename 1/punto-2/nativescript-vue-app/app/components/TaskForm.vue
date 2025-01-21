<template>
    <Page class="ns-light">
      <ActionBar title="Crear Nueva Tarea" />
      <ScrollView>
        <StackLayout padding="20">
          <Label text="Título de la tarea" />
          <TextField v-model="form.title" hint="Título de la tarea" />

          <Label text="Descripción de la tarea" marginTop="10" />
          <TextView v-model="form.description" hint="Descripción de la tarea" />

          <Label text="Fecha de inicio" marginTop="10" />
          <DatePicker
            v-model="form.date_from"
            maxDate="2030-12-31"
          />

          <Label text="Fecha de vencimiento" marginTop="10" />
          <DatePicker
            v-model="form.due_date"
            :minDate="form.date_from"
            maxDate="2030-12-31"
          />

          <Label text="Estatus" marginTop="10" />
          <DropDown
            v-model="form.status_id"
            :items="statuses"
            displayField="name"
            valueField="id"
          />

          <Button
            text="Crear Tarea"
            marginTop="20"
            @tap="createNewTask"
            class="btn btn-primary"
          />
        </StackLayout>
      </ScrollView>
    </Page>
  </template>

  <script lang="ts">
  import Vue from 'nativescript-vue';
  import TaskService from '../services/TaskService';
  import { DatePicker } from '@nativescript/core';
  import DropDown from '@nativescript/dropdown/vue';

  const taskService = new TaskService();

  export default Vue.extend({
    components: { DropDown },
    data() {
      return {
        form: {
          title: '',
          description: '',
          status_id: 1,
          date_from: new Date(), // Fecha inicial por defecto
          due_date: new Date(), // Fecha inicial por defecto
        },
        statuses: [
          { id: 1, name: 'En progreso' },
          { id: 2, name: 'Completada' },
        ],
      };
    },
    methods: {
      async createNewTask() {
        if (!this.form.title || !this.form.description) {
          alert('Por favor, completa todos los campos.');
          return;
        }

        try {
          const taskData = {
            ...this.form,
            // Convierte las fechas a formato ISO para enviarlas
            date_from: this.formatDate(this.form.date_from),
            due_date: this.formatDate(this.form.due_date),
          };

          const newTask = await taskService.createTask(taskData);
          if (newTask) {
            alert('¡Tarea creada exitosamente!');
            // Navegar a otra página o limpiar el formulario
            this.$navigateTo(/* componente o ruta */);
          }
        } catch (error) {
          alert('Error al crear la tarea. Revisa la consola.');
          console.error(error);
        }
      },
      formatDate(date: Date): string {
        const year = date.getFullYear();
        const month = String(date.getMonth() + 1).padStart(2, '0');
        const day = String(date.getDate()).padStart(2, '0');
        return `${year}-${month}-${day}`;
      },
    },
  });
  </script>
