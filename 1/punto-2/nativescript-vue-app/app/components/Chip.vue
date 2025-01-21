<template>
    <Label
      :text="text"
      :style="{ backgroundColor: bgColor, color: textColor }"
      class="chip"
    />
  </template>

  <script lang="ts">
  import Vue from 'nativescript-vue';

  export default Vue.extend({
    props: {
      text: {
        type: String,
        required: true,
      },
      bgColor: {
        type: String,
        default: '#eeeeee', // Color de fondo por defecto
      },
      textColor: {
        type: String,
        default: '#000000', // Color de texto por defecto
      },
    },
  });
  </script>

  <style scoped>
  .chip {
    padding: 20px 25px;
    border-radius: 30px; /* Hace el chip circular */
    font-size: 12px; /* Ajusta el tamaño de la fuente */
    font-weight: bold; /* Texto en negrita */
    text-align: center; /* Centra el texto */
    text-transform: uppercase;
    display: inline-block; /* Evita ocupar toda la fila */
    flex-shrink: 0;
  }
  </style>
