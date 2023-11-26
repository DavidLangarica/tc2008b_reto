from mesa import Agent
from constants import SEED

class Food(Agent):
    def __init__(self, unique_id, model):
        super().__init__(unique_id, model)
        self.is_marked = False
        self.random.seed(SEED)
