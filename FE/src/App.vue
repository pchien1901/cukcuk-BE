<template>
  <div class="container">
    <MSidebar />
    <div :class="computedClasses">
      <MHeader />
      <MMain />
    </div>
  </div>
</template>

<script>
import MHeader from './components/layout/MHeader.vue';
import MSidebar from './components/layout/MSidebar.vue';
import MMain from './components/layout/MMain.vue';

export default {
  name: 'App',
  components: {
    MHeader, MSidebar, MMain
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
