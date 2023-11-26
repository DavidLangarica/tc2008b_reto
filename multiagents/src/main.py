from Restaurant import Restaurant
import random
from constants import *

random.seed(SEED)

def run_simulation():
    model = Restaurant(WIDTH, HEIGHT, NUM_WAITERS, NUM_FOODS)
    while model.is_running:
        model.step()
    return model


def get_grid_size(all_data):
    grid_position = all_data["Grid"][0][0]
    grid_width = grid_position["width"]
    grid_height = grid_position["height"]
    return grid_width, grid_height


def get_bin_position(all_data):
    bin_position = all_data["Bin"][0][0]["position"]
    bin_x = bin_position["x"]
    bin_y = bin_position["y"]
    return bin_x, bin_y


def get_waiters_information(all_data, step):
    num_waiters = all_data["NumWaiters"][0]

    waiters_positions = all_data["Waiters"]
    waiters_data = {}

    for i in range(num_waiters):
        waiter_info = waiters_positions[step][i]
        waiter_id = waiter_info["id"]
        waiter_position = waiter_info["position"]
        waiter_x = waiter_position["x"]
        waiter_y = waiter_position["y"]
        waiter_carrying = waiter_info["carrying_food"]

        if waiter_id not in waiters_data:
            waiters_data[waiter_id] = []

        waiters_data[waiter_id].append(
            {
                "X": waiter_x,
                "Y": waiter_y,
                "CarryingFood": waiter_carrying,
            }
        )
    return waiters_data


def get_food_information(all_data, step):
    food_data = all_data["Food"][step]
    food_info = {
        food["id"]: {"x": food["position"]["x"], "y": food["position"]["y"]}
        for food in food_data
    }

    return food_info

def get_bin_found_position(all_data, step):
    bin_found_position = all_data["isBinFound"][step]
    return bin_found_position != None

def get_data(model: Restaurant):
    all_data = model.datacollector.get_model_vars_dataframe()

    #  --------- Number of steps ---------
    num_steps = len(all_data)
    # print(f"Number of steps: {num_steps}")

    # ---------- Grid size ----------
    grid_size = get_grid_size(all_data)

    # ---------- Bin position ----------
    bin_position = get_bin_position(all_data)

    # ---------- Number of Waiters ----------
    num_waiters = all_data["NumWaiters"][0]
    # print(f"Number of waiters: {num_waiters}")

    # ---------- Waiters information ----------
    waiters_info = get_waiters_information(all_data)

    # ---------- Food information ----------
    food_info = get_food_information(all_data)
    
    # ---------- Bin found position ----------
    bin_found_position = get_bin_found_position(all_data)
    
    return {
        "NumSteps": num_steps,
        "GridSize": grid_size,
        "BinPosition": bin_position,
        "NumWaiters": num_waiters,
        "WaitersInfo": waiters_info,
        "FoodInfo": food_info,
        "isBinFound": bin_found_position,
    }


def main():
    model = run_simulation()
    data = get_data(model)
    return data


if __name__ == "__main__":
    main()
