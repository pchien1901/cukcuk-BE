<template>
  <div class="loading-container">
    <div class="loading-content">
      <div class="loading-content__spinner"></div>
      <div class="loading-content__text">Đang tải ...</div>
    </div>
  </div>
</template>

<script>
export default {
  name: "MLoading",
}
</script>

<style scoped>
.container-right--large .employee-page-container .loading-container {
  left: 56px;
}

.container-right--large .employee-page-container .loading-container .loading-content{
  left:calc(50% - 56px);
}

.loading-container {
  position: fixed;
  width: 100%;
  height: 100%;
  top: 56px;
  left: 200px;
  background-color: #fff;
  /* display: flex;
  align-items: center;
  justify-content: center; */
  z-index: 5;
  background-color: rgba(0, 0, 0, 0.3);
  transition: width 5s ease;
}

.loading-content {
  position: absolute;
  top: calc(50% - 56px);
  left: calc(50% - 200px);
  transform: translate(-50%, -50%);
  opacity: 1;
  /* display: flex;
  flex-direction: column;
  align-items: center; */
}

/* svg {
  width: 40px;
  height: 40px;
} */

.loading-content__spinner {
  border: 4px solid #50b83c;
  border-left-color: #fff;
  border-radius: 50%;
  width: 40px;
  height: 40px;
  animation: spin 1s linear infinite;
  margin: 0px 0px 20px 5px;
}

.loading-content__text {
  font-size: 14px;
  font-family: "Roboto",serif;
  color: #fff;
  font-weight: 500;
}

@keyframes spin {
  0% {
    transform: rotate(0deg);
  }
 
  100% {
    transform: rotate(360deg);
  }
}
</style>