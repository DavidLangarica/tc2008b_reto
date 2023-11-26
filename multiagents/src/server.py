from flask import Flask, jsonify
from Restaurant import Restaurant
from main import *
from constants import *

app = Flask(__name__)
model = None 

def initialize_model():
    global model
    model = Restaurant(WIDTH, HEIGHT, NUM_WAITERS, NUM_FOODS)
    model.step()

@app.route("/init", methods=["GET"])
def init():
    initialize_model()
    model_data = model.datacollector.get_model_vars_dataframe()

    # GRID INIT
    grid_data = get_grid_size(model_data)
    grid_width = grid_data[0]
    grid_height = grid_data[1]

    # BIN INIT
    bin_data = get_bin_position(model_data)
    bin_x = bin_data[0]
    bin_y = bin_data[1]

    # WAITERS INIT
    waiters_data = get_waiters_information(model_data, 0)

    # FOOD INIT
    food_data = get_food_information(model_data, 0)

    # Return
    return jsonify(
        {
            "Grid": {"width": grid_width, "height": grid_height},
            "Bin": {"x": bin_x, "y": bin_y},
            "Waiters": waiters_data,
            "Food": food_data,
        }
    )

@app.route("/step", methods=["GET"])
def step():
    global model
    if model is None:
        return jsonify({"message": "Model is not initialized"})

    model.step()
    model_data = model.datacollector.get_model_vars_dataframe()
    current_step = len(model_data) - 1
    waiters_data = get_waiters_information(model_data, current_step)
    food_data = get_food_information(model_data, current_step)
    is_bin_found = get_bin_found_position(model_data, current_step)
    model_is_running = model.is_running

    return jsonify(
        {
            "currentStep": current_step,
            "Waiters": waiters_data,
            "Food": food_data,
            "isBinFound": is_bin_found,
            "modelIsRunning": model_is_running,
        }
    )

@app.route("/food", methods=["GET"])
def generate_food():
    global model
    if model is None:
        return jsonify({"message": "Model is not initialized"})

    model.create_foods()  # Make sure this method exists in your model
    return jsonify({"message": "Food generated successfully"})

if __name__ == "__main__":
    app.run(debug=True)
