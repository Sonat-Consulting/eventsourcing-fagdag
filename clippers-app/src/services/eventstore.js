import eventstoreApi from "./eventstoreApi";

export const startHaircut = async (HaircutId, HairdresserId) => {
    try {
        const StartedAt = new Date();
      const response = await eventstoreApi.post("/startHaircut",
        {
            HaircutId,
            StartedAt,
            HairdresserId
        }
      );
    } catch (error) {
      console.error(error);
    }
};

export const completeHaircut = async (HaircutId) => {
    try {
        const CompletedAt = new Date();
      const response = await eventstoreApi.post("/completeHaircut",
        {
            HaircutId,
            CompletedAt,
        }
      );
    } catch (error) {
      console.error(error);
    }
};
