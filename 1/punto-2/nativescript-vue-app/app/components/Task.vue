<!-- app/components/Home.vue -->

<template>
    <Page class="ns-light">
        <ActionBar title="Tareas" class="ActionBar">
  <ActionItem
    ios.systemIcon="4"
    ios.position="right"
    text="Añadir"
    android.position="popup"
    @tap="navigateToCreateTask"
  />
</ActionBar>

      <ListView v-if="tasks.length > 0" padding="10" height="100%" separatorColor="transparent" for="item in tasks">
        <v-template>
          <GridLayout
            borderRadius="20"
            class="bg-secondary card"
            rows="auto,auto,auto"
            columns="*"
            margin="10"
            padding="0"
             :isUserInteractionEnabled="false"
          >
            <Label
              row="0"
              margin="10 10 0 10"
              fontWeight="700"
              class="text-primary"
              fontSize="18"
              :text="item.title"
            />
            <Label
              row="1"
              margin="0 10 0 10"
              class="text-secondary"
              fontSize="14"
              textAlignment="initial"
              textWrap="true"
              :text="item.description"
            />
            <StackLayout row="2" orientation="horizontal"  margin="0,0,10,10">
            <Chip
              :text="item.status.name"
              :bgColor="getStatusBgColor(item.status.id)"
              :textColor="getStatusTextColor(item.status.id)"
            />
          </StackLayout>
          </GridLayout>
        </v-template>
      </ListView>

    <StackLayout verticalAlignment="center" horizontalAlignment="center" margin="20">
        <Label text="Título del Stack" fontSize="20" fontWeight="bold" textAlignment="center" />
        <Label text="Descripción del Stack" fontSize="16" textAlignment="center" />
    </StackLayout>

      <Label  v-if="tasks.length == 0 && !busy" text="No hay tareas, agrega una y aparecerá aquí"  fontSize="15" padding="5" textAlignment="center"  />
        <ActivityIndicator :busy="busy" />
    </Page>
  </template>

  <script lang="ts">
  import Vue from 'nativescript-vue'
  import TaskService from '../services/TaskService'
  import TaskModel from '../models/Task'
  import Chip from './Chip.vue';
  const taskService = new TaskService()

  export default Vue.extend({
    components: { Chip },
    data() {
      return {
        tasks:[] as TaskModel[],
        busy: false
      }
    },
    methods: {
    /**
     * Devuelve el color de fondo según el status.id
     */
     navigateToCreateTask() {
    this.$navigateTo(require('./TaskForm.vue').default);
  },
    getStatusBgColor(statusId: number): string {
      switch (statusId) {
        case 1: // Naranja
          return '#FFE4B5'; // Naranja claro
        case 2: // Verde
          return '#C8E6C9'; // Verde claro
        default:
          return '#eeeeee'; // Color predeterminado
      }
    },
    /**
     * Devuelve el color de texto según el status.id
     */
    getStatusTextColor(statusId: number): string {
      switch (statusId) {
        case 1: // Naranja
          return '#FF8C00'; // Naranja oscuro
        case 2: // Verde
          return '#388E3C'; // Verde oscuro
        default:
          return '#000000'; // Color predeterminado (negro)
      }
    },
  },

    async created() {
        try {
            this.busy = true;
            this.tasks = await taskService.getTasks();
            // alert("length:"+this.tasks.length + "  task:"+this.tasks[0].title);
        } catch (error) {
            alert('Error fetching tasks:', error);
        } finally {
            this.busy = false;
        }
    }
 })
  </script>
