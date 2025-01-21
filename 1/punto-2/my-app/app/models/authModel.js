import { ApplicationSettings } from '@nativescript/core';

const apiUrl = 'http://127.0.0.1:8000/auth/login';

export default {
  async login(email, password) {
    try {
      const response = await Http.request({
        url: apiUrl,
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        content: JSON.stringify({ email, password }),
      });

      if (response.statusCode === 200) {
        const data = await response.content.toJSON();
        const token = data.token; // Suponiendo que la respuesta incluye un token
        // ApplicationSettings.setString('token', token); // Guardar token
        console.log(token);
        return true;
      } else {
        return false;
      }
    } catch (error) {
      console.error(error);
      return false;
    }
  },

  isLoggedIn() {
    // const token = ApplicationSettings.getString('token'); // Obtener el token
    const token ="";
    return token !== undefined && token !== ''; // Verificar si el token existe
  },
};
