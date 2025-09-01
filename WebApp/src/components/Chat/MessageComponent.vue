<template>
  <div 
    class="message-wrapper"
    :class="{ 'message-sender': isSender, 'message-receiver': !isSender }"
  >
    <div 
      class="message-bubble"
      :class="{ 
        'sender-bubble': isSender, 
        'receiver-bubble': !isSender 
      }"
    >
      <p class="message-text">{{ message }}</p>
      <div class="message-meta">
        <span class="message-time">{{ formatTime(timestamp) }}</span>
        <v-icon 
          v-if="isSender" 
          size="12" 
          :color="isSender ? 'white' : 'grey'"
          class="ml-1"
        >
          mdi-check
        </v-icon>
      </div>
    </div>
  </div>
</template>

<script lang='ts'>
import { defineComponent } from 'vue'

export default defineComponent({
  name: 'MessageComponent',
  props: {
    message: {
      type: String,
      required: true
    },
    isSender: {
      type: Boolean,
      required: true
    },
    timestamp: {
      type: [Date, String],
      default: () => new Date()
    }
  },
  methods: {
    formatTime(date: Date | string) {

      let dateObj: Date;
      
      if (typeof date === 'string') {
        // Parse UTC date string and ensure it's treated as UTC
        dateObj = new Date(date + (date.includes('Z') ? '' : 'Z'));
      } else {
        dateObj = date;
      }
      
      return dateObj.toLocaleTimeString([], { 
        hour: '2-digit', 
        minute: '2-digit',
        hour12: false 
      });
    }
  }
})
</script>

<style scoped>
.message-wrapper {
  display: flex;
  margin-bottom: 8px;
  max-width: 100%;
}

.message-sender {
  justify-content: flex-end;
}

.message-receiver {
  justify-content: flex-start;
}

.message-bubble {
  max-width: 75%;
  padding: 12px 16px 8px;
  border-radius: 18px;
  position: relative;
  word-wrap: break-word;
  animation: messageAppear 0.2s ease-out;
}

.sender-bubble {
  background: linear-gradient(135deg, #8B0000 0%, #A52A2A 100%);
  color: white;
  border-bottom-right-radius: 4px;
  box-shadow: 0 2px 8px rgba(139, 0, 0, 0.3);
}

.receiver-bubble {
  background: #FFFFFF;
  color: #212121;
  border-bottom-left-radius: 4px;
  box-shadow: 0 2px 8px rgba(0, 0, 0, 0.1);
  border: 1px solid rgba(0, 0, 0, 0.05);
}

.message-text {
  margin: 0;
  line-height: 1.4;
  font-size: 14px;
  font-weight: 400;
  word-break: break-word;
}

.message-meta {
  display: flex;
  align-items: center;
  justify-content: flex-end;
  margin-top: 4px;
  opacity: 0.8;
}

.message-time {
  font-size: 11px;
  font-weight: 500;
}

.sender-bubble .message-time {
  color: rgba(255, 255, 255, 0.9);
}

.receiver-bubble .message-time {
  color: rgba(33, 33, 33, 0.6);
}

/* Message bubble tails */
.sender-bubble::after {
  content: '';
  position: absolute;
  bottom: 0;
  right: -6px;
  width: 0;
  height: 0;
  border: 6px solid transparent;
  border-left-color: #A52A2A;
  border-bottom: none;
  border-top-left-radius: 4px;
}

.receiver-bubble::after {
  content: '';
  position: absolute;
  bottom: 0;
  left: -6px;
  width: 0;
  height: 0;
  border: 6px solid transparent;
  border-right-color: #FFFFFF;
  border-bottom: none;
  border-top-right-radius: 4px;
}

/* Animations */
@keyframes messageAppear {
  from {
    opacity: 0;
    transform: translateY(10px) scale(0.95);
  }
  to {
    opacity: 1;
    transform: translateY(0) scale(1);
  }
}

/* Hover effects */
.message-bubble:hover {
  transform: translateY(-1px);
  transition: transform 0.2s ease;
}

.sender-bubble:hover {
  box-shadow: 0 4px 12px rgba(139, 0, 0, 0.4);
}

.receiver-bubble:hover {
  box-shadow: 0 4px 12px rgba(0, 0, 0, 0.15);
}

/* Long message handling */
.message-text {
  overflow-wrap: break-word;
  hyphens: auto;
}

/* Mobile adjustments */
@media (max-width: 600px) {
  .message-bubble {
    max-width: 85%;
    padding: 10px 14px 6px;
  }
  
  .message-text {
    font-size: 13px;
  }
  
  .message-time {
    font-size: 10px;
  }
}
</style>
