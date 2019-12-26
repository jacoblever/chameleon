class Config {
  static backendBaseApiUrl() {
    return process.env.REACT_APP_CHAMELEON_BACKEND_BASE_URL + "api-2/";
  }

  static personIdHeader() {
    return "x-chameleon-personid";
  }
}

export default Config;
