import { fileURLToPath, URL } from 'node:url'

import { defineConfig } from 'vite'
import vue from '@vitejs/plugin-vue'
// Comment out vite-plugin-vue-devtools temporarily to fix module loading issues
// import vueDevTools from 'vite-plugin-vue-devtools'

// https://github.com/vuetifyjs/vuetify-loader/tree/next/packages/vite-plugin
import vuetify from 'vite-plugin-vuetify'

// https://vite.dev/config/
export default defineConfig({
  plugins: [
    vue({
      template: {
        compilerOptions: {
          // Preserve Unicode characters during compilation
          whitespace: 'preserve'
        }
      }
    }),
    // Temporarily disable vue-devtools to fix module loading
    // vueDevTools(),
    vuetify({
      autoImport: true,

    })
  ],
  resolve: {
    alias: {
      '@': fileURLToPath(new URL('./src', import.meta.url))
    },
  },
  // Ensure UTF-8 encoding is preserved
  define: {
    __VUE_PROD_DEVTOOLS__: false,
  },
  build: {
    // Preserve character encoding in build
    rollupOptions: {
      output: {
        // Preserve Unicode in output
        assetFileNames: 'assets/[name]-[hash][extname]',
        chunkFileNames: 'assets/[name]-[hash].js',
        entryFileNames: 'assets/[name]-[hash].js'
      }
    }
  },
  server: {
    // Remove problematic server headers that cause MIME type issues
    port: 5173,
    host: true,
  }
})
