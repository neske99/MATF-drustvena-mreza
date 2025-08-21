import 'vuetify/styles'
import '@mdi/font/css/materialdesignicons.css'
import { createVuetify } from 'vuetify'

const vuetify = createVuetify({
  icons: {
    defaultSet: 'mdi', // Simple MDI font icon setup
  },
  theme: {
    defaultTheme: 'light',
    themes: {
      light: {
        colors: {
          // MATF University colors - dark red theme like official website
          primary: '#8B0000',           // Dark red (main MATF color)
          'matf-red': '#8B0000',        // Custom MATF dark red
          'matf-dark-red': '#660000',   // Even darker red for contrast
          'matf-light-red': '#A52A2A',  // Lighter red for accents
          secondary: '#424242',          // Academic gray
          success: '#2E7D32',           // Academic green
          info: '#0277BD',              // Information blue
          warning: '#F57C00',           // Academic orange
          error: '#C62828',             // Error red
          background: '#FAFAFA',        // Light academic background
          surface: '#FFFFFF',           // White surface
          'on-primary': '#FFFFFF',
          'on-secondary': '#FFFFFF',
          'on-success': '#FFFFFF',
          'on-error': '#FFFFFF',
          // Academic accent colors
          'academic-gold': '#FFB300',
          'academic-navy': '#1A237E',
          'text-primary': '#212121',
          'text-secondary': '#757575',
        },
      },
      dark: {
        colors: {
          primary: '#A52A2A',
          'matf-red': '#A52A2A',
          'matf-dark-red': '#8B0000',
          'matf-light-red': '#CD5C5C',
          secondary: '#616161',
          success: '#4CAF50',
          info: '#03DAC6',
          warning: '#FF9800',
          error: '#CF6679',
          background: '#121212',
          surface: '#1E1E1E',
          'academic-gold': '#FFC107',
          'academic-navy': '#3F51B5',
        },
      },
    },
  },
  defaults: {
    VBtn: {
      style: 'text-transform: none; letter-spacing: normal; font-family: "Noto Sans", "Inter", "Open Sans", system-ui, sans-serif;',
    },
    VCard: {
      elevation: 2,
    },
    VTextField: {
      variant: 'outlined' as const,
      density: 'comfortable' as const,
      style: 'font-family: "Noto Sans", "Inter", "Open Sans", system-ui, sans-serif;',
    },
    VTextarea: {
      style: 'font-family: "Noto Sans", "Inter", "Open Sans", system-ui, sans-serif;',
    },
    VSelect: {
      style: 'font-family: "Noto Sans", "Inter", "Open Sans", system-ui, sans-serif;',
    },
    VLabel: {
      style: 'font-family: "Noto Sans", "Inter", "Open Sans", system-ui, sans-serif;',
    },
  },
})

export default vuetify

