from mesa import Agent


class Food(Agent):
    def __init__(self, unique_id, model):
        super().__init__(unique_id, model)
        self.is_marked = False
        self.random.seed(12345)
