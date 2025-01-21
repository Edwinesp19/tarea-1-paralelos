<template>
    <Page>
      <ActionBar title="Login">
        <Label slot="title">Login</Label>
      </ActionBar>

      <GridLayout>
        <StackLayout padding="20">
          <TextField v-model="email" hint="Correo electrónico" keyboardType="email" />
          <SecureTextField v-model="password" hint="Contraseña" />

          <Button text="Iniciar sesión" @tap="login" />

          <Label v-if="errorMessage" color="red" :text="errorMessage" />
        </StackLayout>
      </GridLayout>
    </Page>
  </template>

  <script>
  import { createComponent } from 'vue';
  import LoginViewModel from '../viewModel/loginViewModel';

  export default {
    data() {
      return {
        viewModel: new LoginViewModel()
      };
    },
    computed: {
      email: {
        get() {
          return this.viewModel.email;
        },
        set(value) {
          this.viewModel.email = value;
        }
      },
      password: {
        get() {
          return this.viewModel.password;
        },
        set(value) {
          this.viewModel.password = value;
        }
      },
      errorMessage: {
        get() {
          return this.viewModel.errorMessage;
        },
        set(value) {
          this.viewModel.errorMessage = value;
        }
      }
    },
    methods: {
      login() {
        this.viewModel.login();
      }
    }
  };
  </script>

