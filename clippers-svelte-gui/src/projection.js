import projectionApi from "./projectionApi";

export const getQueueView = async () => {
    try {
      const response = await projectionApi.get("/projections/QueueView");
      //console.log(response);
      let json = JSON.parse(response)
      return json.payload.Customers;
    } catch (error) {
      console.error(error);
    }
};
