from mesa import Model
from mesa.space import MultiGrid
from mesa.time import RandomActivation
from mesa.datacollection import DataCollector
from Waiter import Waiter
from Bin import Bin
from Food import Food
import random
import numpy as np
import time

random.seed(12345)


def get_grid_size(model):
    grid_data = [
        {
            "id": "grid",
            "width": model.grid.width,
            "height": model.grid.height,
        }
    ]
    return grid_data


def get_bin_position(model):
    bin_data = [
        {
            "id": agent.unique_id,
            "position": {"x": agent.pos[0], "y": agent.pos[1]},
        }
        for agent in model.schedule.agents
        if isinstance(agent, Bin)
    ]
    return bin_data


def get_waiter_position(model):
    waiters_data = [
        {
            "id": agent.unique_id,
            "position": {"x": agent.pos[0], "y": agent.pos[1]},
            # Conditional for carrying food "true" == 1, "false" == 0
            "carrying_food": 1 if agent.carrying_food else 0,
        }
        for agent in model.schedule.agents
        if isinstance(agent, Waiter)
    ]
    return waiters_data


def get_food_position(model):
    food_data = [
        {
            "id": agent.unique_id,
            "position": {"x": agent.pos[0], "y": agent.pos[1]},
        }
        for agent in model.schedule.agents
        if isinstance(agent, Food)
    ]
    return food_data


def track_food_picking(model):
    food_picking_steps = {}
    for agent in model.schedule.agents:
        if isinstance(agent, Waiter):
            food_picking_steps[agent.unique_id] = agent.food_picking_steps
    return food_picking_steps


class Restaurant(Model):
    def __init__(self, width, height, num_waiters, num_foods):
        random.seed(12345)
        self.num_waiters = num_waiters
        self.num_foods = num_foods
        self.total_food = num_foods
        self.schedule = RandomActivation(self)
        self.grid = MultiGrid(width, height, False)
        self.datacollector = DataCollector(
            model_reporters={
                "Grid": get_grid_size,
                "Bin": get_bin_position,
                "Food": get_food_position,
                "NumWaiters": lambda x: num_waiters,
                "Waiters": get_waiter_position,
                "FoodPicking": track_food_picking,
            }
        )

        self.matrix = np.zeros((width, height))
        self.food_id = 0
        self.last_food_creation_time = time.time()
        self.column_width = None
        self.bin_position = None
        self.marked_food = []
        self.food_placed = 0
        self.is_running = True
        self.agent_in_bin = False

        self.create_waiters()
        self.create_bin()

    def random_position(self):
        x = random.randrange(self.grid.width)
        y = random.randrange(self.grid.height)
        return (x, y)

    def create_bin(self):
        bin_id = "bin"
        x, y = self.random_position()
        bin = Bin(bin_id, self)
        self.schedule.add(bin)
        self.grid.place_agent(bin, (x, y))

    def create_foods(self):
        random.seed(12345)
        food_unit = random.randint(2, 5)

        if food_unit > self.num_foods:
            food_unit = self.num_foods

        for i in range(food_unit):
            food_id = "food-" + str(self.food_id)
            while True:
                x, y = self.random_position()
                if self.grid.is_cell_empty((x, y)):
                    food = Food(food_id, self)
                    self.schedule.add(food)
                    self.grid.place_agent(food, (x, y))
                    self.food_id += 1
                    break
        self.num_foods -= food_unit

    def create_waiters(self):
        random.seed(12345)
        self.column_width = self.grid.width // self.num_waiters
        column = 0
        for i in range(self.num_waiters):
            waiter_id = "waiter-" + str(i)
            while True:
                x = random.randrange(column, column + self.column_width - 1)
                x_range = (column, column + self.column_width - 1)
                y = random.randrange(self.grid.height)
                if self.grid.is_cell_empty((x, y)):
                    waiter = Waiter(waiter_id, self, x_range)
                    self.schedule.add(waiter)
                    self.grid.place_agent(waiter, (x, y))
                    break
            column += self.column_width

    def simulation_finished(self):
        if self.food_placed == self.total_food:
            self.is_running = False

    def step(self):
        self.datacollector.collect(self)
        self.schedule.step()
        self.simulation_finished()
