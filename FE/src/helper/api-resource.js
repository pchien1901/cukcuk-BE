const MApiResource = {
  apiUrl: {
    login: "Authenticate/login",
    register: "register",
    registerAdmin: "register-admin",
    refreshToken: "refresh-token",
    revoke: "revoke",
  },
  apiMethod: {
    get: "get",
    post: "post",
    put: "put",
    delete: "delete",
  },
  apiHeaderContentType: {
    jsonType: "json",
    applicationType: "application/json",
    arrayType: "arraybuffer",
    formData: "multipart/form-data",
  },
};

export default MApiResource;