class Non200ResponseError extends Error {
  constructor(response) {
    super(`Backend returned status ${response.status}`);
    this.response = response;
  }
}

export default Non200ResponseError;