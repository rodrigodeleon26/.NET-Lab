/** @type {import('tailwindcss').Config} */
module.exports = {
  content: [
    "./src/**/*.{html,ts}",
  ],
  theme: {
    extend: {
      fontFamily: {
        nunito: ['Nunito', 'sans-serif'],
      },
      colors: {
        gray: {
          claro: '#F2F2F2',
          intermedio: '#BDBDBD',
          oscuro: '#4A4A4A',
        },
        blue: {
          claro: '#88C9F9',
          intermedio: '#5A9BD5',
          oscuro: '#2A6F97',
        },
        green: {
          claro: '#B2E3B6',
          intermedio: '#7CCB8A',
          oscuro: '#3B7A57',
        },
        amarillo: {
          claro: '#FFF8B0',
        },
      },
    },
  },
  plugins: [],
}