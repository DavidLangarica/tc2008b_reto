from mesa import Agent


class Bin(Agent):
    def __init__(self, unique_id, model):
        super().__init__(unique_id, model)
        self.random.seed(12345)
