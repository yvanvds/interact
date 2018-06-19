{
  "object 0": {
    "ID": 63,
    "gui": {
      "height": "25",
      "pos_x": "97",
      "pos_y": "123",
      "width": "150"
    },
    "outputs": {
      "output 0": {
        "0": {
          "Inlet": 0,
          "Object": 65
        },
        "Count": 1
      }
    },
    "parms": "",
    "type": ".slider"
  },
  "object 1": {
    "ID": 67,
    "gui": {
      "height": "42",
      "pos_x": "197",
      "pos_y": "64",
      "width": "132"
    },
    "parms": "sliders accept a float input between 0 and 1",
    "type": ".text"
  },
  "object 2": {
    "ID": 66,
    "gui": {
      "height": "68",
      "pos_x": "90",
      "pos_y": "224",
      "width": "162"
    },
    "outputs": {
      "output 0": {
        "Count": 0
      }
    },
    "parms": "",
    "type": ".slider"
  },
  "object 3": {
    "ID": 65,
    "gui": {
      "pos_x": "90",
      "pos_y": "169"
    },
    "outputs": {
      "output 0": {
        "0": {
          "Inlet": 0,
          "Object": 66
        },
        "Count": 1
      }
    },
    "parms": "0.5",
    "type": ".*"
  },
  "object 4": {
    "ID": 69,
    "gui": {
      "height": "48",
      "pos_x": "269",
      "pos_y": "225",
      "width": "88"
    },
    "parms": "sliders can be resized",
    "type": ".text"
  },
  "object 5": {
    "ID": 68,
    "gui": {
      "height": "48",
      "pos_x": "260",
      "pos_y": "124",
      "width": "135"
    },
    "parms": "output value is triggered on mouse click, drag and wheel",
    "type": ".text"
  },
  "object 6": {
    "ID": 64,
    "gui": {
      "pos_x": "97",
      "pos_y": "67"
    },
    "outputs": {
      "output 0": {
        "0": {
          "Inlet": 0,
          "Object": 63
        },
        "Count": 1
      }
    },
    "parms": "0",
    "type": ".f"
  }
}