import { Observable } from '@nativescript/core';
import authModel from '../models/authModel';

export default class LoginViewModel extends Observable {
  constructor() {
    super();
    this.email = '';
    this.password = '';
    this.errorMessage = '';
  }

  // Función para manejar el envío del formulario
  async login() {
    const success = await authModel.login(this.email, this.password);
    if (success) {
      // this.set('errorMessage', ''); // Limpiar mensajes de error
      this.navigateHome();
    } else {
      // this.set('errorMessage', 'Correo o contraseña incorrectos');
    }
  }

  navigateHome() {
    const topmost = require('@nativescript/core').Topmost;
    topmost().navigate({
      moduleName: 'components/Home',
    });
  }
}
