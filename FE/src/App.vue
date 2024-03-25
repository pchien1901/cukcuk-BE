<template>
  <div class="container">
    <TheSidebar />
    <div :class="computedClasses">
      <TheHeader />
      <TheMain />
    </div>
  </div>
</template>

<script>
import TheHeader from './components/layout/TheHeader.vue';
import TheSidebar from './components/layout/TheSidebar.vue';
import TheMain from './components/layout/TheMain.vue';

export default {
  name: 'App',
  components: {
    TheHeader, TheSidebar, TheMain
  },
  created() {
    this.$tinyEmitter.on("lessSidebar", this.handleLessSidebar);
  },
  beforeUnmount() {
    this.$tinyEmitter.off("lessSidebar");
  },
  data() {
    return {
      isLarge: false,
    }
  },
  computed: {
    computedClasses() {
      return {
        "container-right": true,
        "container-right--large": this.isLarge,
      };
    }
  },
  methods: {
    handleLessSidebar(isLess) {
      this.isLarge = isLess;
    }
  }
}
</script>

<style>
@import url('./css/layout/main-layout.css');
</style>
