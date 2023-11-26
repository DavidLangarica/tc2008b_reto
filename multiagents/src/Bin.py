from mesa import Agent
from constants import SEED

class Bin(Agent):
    def __init__(self, unique_id, model):
        super().__init__(unique_id, model)
        self.random.seed(SEED)
