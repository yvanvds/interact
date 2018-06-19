{
  "object 0": {
    "ID": 19,
    "gui": {
      "pos_x": "96",
      "pos_y": "143"
    },
    "outputs": {
      "output 0": {
        "0": {
          "Inlet": 0,
          "Object": 24
        },
        "Count": 1
      }
    },
    "parms": "3",
    "type": ".switch"
  },
  "object 1": {
    "ID": 24,
    "gui": {
      "pos_x": "96",
      "pos_y": "207"
    },
    "outputs": {
      "output 0": {
        "Count": 0
      }
    },
    "parms": "",
    "type": ".f"
  },
  "object 2": {
    "ID": 23,
    "gui": {
      "pos_x": "314",
      "pos_y": "84"
    },
    "outputs": {
      "output 0": {
        "0": {
          "Inlet": 3,
          "Object": 19
        },
        "Count": 1
      }
    },
    "parms": "500",
    "type": ".f"
  },
  "object 3": {
    "ID": 25,
    "gui": {
      "height": "51",
      "pos_x": "21",
      "pos_y": "19",
      "width": "97"
    },
    "parms": "left inlet determines the active inlet ",
    "type": ".text"
  },
  "object 4": {
    "ID": 26,
    "gui": {
      "height": "60",
      "pos_x": "189",
      "pos_y": "144",
      "width": "150"
    },
    "parms": "argument sets the number of inlets (+ 1 for the selector)",
    "type": ".text"
  },
  "object 5": {
    "ID": 20,
    "gui": {
      "pos_x": "19",
      "pos_y": "83"
    },
    "outputs": {
      "output 0": {
        "0": {
          "Inlet": 0,
          "Object": 19
        },
        "Count": 1
      }
    },
    "parms": "",
    "type": ".i"
  },
  "object 6": {
    "ID": 21,
    "gui": {
      "pos_x": "144",
      "pos_y": "83"
    },
    "outputs": {
      "output 0": {
        "0": {
          "Inlet": 1,
          "Object": 19
        },
        "Count": 1
      }
    },
    "parms": "10",
    "type": ".f"
  },
  "object 7": {
    "ID": 22,
    "gui": {
      "pos_x": "229",
      "pos_y": "83"
    },
    "outputs": {
      "output 0": {
        "0": {
          "Inlet": 2,
          "Object": 19
        },
        "Count": 1
      }
    },
    "parms": "100",
    "type": ".f"
  },
  "object 8": {
    "ID": 27,
    "gui": {
      "height": "56",
      "pos_x": "26",
      "pos_y": "272",
      "width": "142"
    },
    "parms": "switch can be used with numbers, strings, lists and triggers",
    "type": ".text"
  }
}