const MApiResource = {
  apiUrl: {
    login: "Authenticate/login",
    register: "Authenticate/register",
    registerAdmin: "Authenticate/register-admin",
    refreshToken: "Authenticate/refresh-token",
    revoke: "Authenticate/revoke",
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