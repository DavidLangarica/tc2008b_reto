from mesa import Agent
from Bin import Bin
from Food import Food
from constants import SEED

class Waiter(Agent):
    def __init__(self, unique_id, model, x_range):
        super().__init__(unique_id, model)
        self.random.seed(SEED)
        self.x_range = x_range
        self.carrying_food = False
        self.in_column = True
        self.horizontal_direction = 1
        self.vertical_direction = 1
        self.current_x = x_range[0]
        self.food_picking_steps = []

    def move(self):
        if self.model.bin_position == None and self.carrying_food == False:
            self.search_bin()
        elif (
            self.carrying_food == True
            and self.model.bin_position != None
            and self.model.agent_in_bin == False
        ):
            self.move_to_target(self.model.bin_position)
        else:
            self.search_food()
            
    def is_cell_free(self, pos):
        cell_contents = self.model.grid.get_cell_list_contents(pos)
        return not any(
            isinstance(agent, Waiter) for agent in cell_contents
        )

    def search_routine(self):
        # Moverse en el eje X
        new_x = self.pos[0] + self.horizontal_direction

        # Verificar si ha llegado al final o al principio del rango de columna
        if new_x > self.x_range[1] or new_x < self.x_range[0]:
            self.horizontal_direction *= -1  # Cambiar dirección horizontal
            new_x = self.pos[0]  # Mantenerse en la misma columna
            new_y = (
                self.pos[1] + self.vertical_direction
            )  # Moverse en la dirección vertical

            # Verificar si ha llegado al final o principio del grid verticalmente
            if new_y >= self.model.grid.height or new_y < 0:
                self.vertical_direction *= -1  # Cambiar dirección vertical
                new_y = (
                    self.pos[1] + self.vertical_direction
                )  # Moverse en la nueva dirección vertical
        else:
            new_y = self.pos[1]

        # Moverse a la nueva posición
        new_position = (new_x, new_y)
        
        # Verificar si la nueva posición está libre
        if self.is_cell_free(new_position):
            self.model.grid.move_agent(self, new_position)
        else:
            pass
        

    def search_bin(self):
        current_cell_contents = self.model.grid.get_cell_list_contents(self.pos)
        for agent in current_cell_contents:
            if isinstance(
                agent, Bin
            ):  # Suponiendo que Bin es una clase para los agentes contenedor
                self.model.bin_position = self.pos
                return
            elif isinstance(agent, Food):
                if agent.is_marked == False:
                    agent.is_marked = True
                    self.model.matrix[self.pos[0]][self.pos[1]] = 1
                    self.model.marked_food.append(agent.pos)

        self.search_routine()

    def pick_food(self, agent):
        self.model.grid.remove_agent(agent)
        self.model.schedule.remove(agent)
        self.model.matrix[self.pos[0]][self.pos[1]] = 0
        self.food_picking_steps.append(self.model.schedule.steps)

    def search_food(self):
        # Regresa a su rango de columnas si se sale de él
        if self.pos[0] not in range(self.x_range[0], self.x_range[1] + 1):
            self.move_to_target((self.x_range[0], self.pos[1]))
            return

        current_cell_contents = self.model.grid.get_cell_list_contents(self.pos)
        for agent in current_cell_contents:
            if isinstance(agent, Food):
                if agent.is_marked == False:
                    agent.is_marked = True
                    self.model.matrix[self.pos[0]][self.pos[1]] = 1
                    self.model.marked_food.append(agent.pos)
                self.carrying_food = True
                self.pick_food(agent)

        self.search_routine()

    def distance_to_target(self, pos, target_pos):
        distance_x, distance_y = pos[0] - target_pos[0], pos[1] - target_pos[1]
        return abs(distance_x) + abs(distance_y)

    def find_nearest_food(self):
        foods = [food for food in self.model.marked_food]
        if not foods:
            return None

        closest = min(foods, key=lambda food: self.distance_to_target(self.pos, food))
        return closest

    def move_to_target(self, target_pos):
        if self.pos == self.model.bin_position and self.carrying_food == True:
            self.place_food()
            self.model.agent_in_bin = False
            return

        if self.pos == target_pos and self.carrying_food == False:
            current_cell_contents = self.model.grid.get_cell_list_contents(self.pos)
            for agent in current_cell_contents:
                if isinstance(agent, Food):
                    self.carrying_food = True
                    self.pick_food(agent)
            return

        current_cell_contents = self.model.grid.get_cell_list_contents(self.pos)
        for agent in current_cell_contents:
            if isinstance(agent, Food):
                if agent.is_marked == False:
                    agent.is_marked = True
                    self.model.matrix[self.pos[0]][self.pos[1]] = 1
                    self.model.marked_food.append(agent.pos)

        possible_steps = self.model.grid.get_neighborhood(
            self.pos, moore=False, include_center=False
        )

        possible_steps = [
            step
            for step in possible_steps
            if self.is_cell_free(step)
        ]

        best_step = min(
            possible_steps, key=lambda pos: self.distance_to_target(pos, target_pos)
        )

        self.model.grid.move_agent(self, best_step)

    def place_food(self):
        self.model.agent_in_bin = True
        self.model.grid.place_agent(self, self.model.bin_position)
        self.carrying_food = False
        self.model.food_placed += 1

    def step(self):
        self.move()
