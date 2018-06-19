{
  "object 0": {
    "ID": 50,
    "gui": {
      "height": "25",
      "pos_x": "158",
      "pos_y": "75",
      "width": "150"
    },
    "outputs": {
      "output 0": {
        "0": {
          "Inlet": 0,
          "Object": 54
        },
        "Count": 1
      }
    },
    "parms": "",
    "type": ".slider"
  },
  "object 1": {
    "ID": 53,
    "gui": {
      "height": "25",
      "pos_x": "163",
      "pos_y": "182",
      "width": "150"
    },
    "parms": "Set Volume",
    "type": ".text"
  },
  "object 2": {
    "ID": 49,
    "gui": {
      "pos_x": "40",
      "pos_y": "207"
    },
    "outputs": {
      "output 0": {
        "0": {
          "Inlet": 0,
          "Object": 48
        },
        "Count": 1
      }
    },
    "parms": "0",
    "type": "~*"
  },
  "object 3": {
    "ID": 51,
    "gui": {
      "height": "25",
      "pos_x": "159",
      "pos_y": "209",
      "width": "150"
    },
    "outputs": {
      "output 0": {
        "0": {
          "Inlet": 1,
          "Object": 49
        },
        "Count": 1
      }
    },
    "parms": "",
    "type": ".slider"
  },
  "object 4": {
    "ID": 48,
    "gui": {
      "pos_x": "40",
      "pos_y": "261"
    },
    "parms": "",
    "type": "~dac"
  },
  "object 5": {
    "ID": 56,
    "gui": {
      "pos_x": "40",
      "pos_y": "70"
    },
    "outputs": {
      "output 0": {
        "0": {
          "Inlet": 0,
          "Object": 57
        },
        "Count": 1
      }
    },
    "parms": "",
    "type": "~noise"
  },
  "object 6": {
    "ID": 55,
    "gui": {
      "height": "25",
      "pos_x": "159",
      "pos_y": "43",
      "width": "150"
    },
    "parms": "Set lowpass frequency",
    "type": ".text"
  },
  "object 7": {
    "ID": 54,
    "gui": {
      "pos_x": "158",
      "pos_y": "115"
    },
    "outputs": {
      "output 0": {
        "0": {
          "Inlet": 1,
          "Object": 57
        },
        "Count": 1
      }
    },
    "parms": "10000",
    "type": ".*"
  },
  "object 8": {
    "ID": 57,
    "outputs": {
      "output 0": {
        "0": {
          "Inlet": 0,
          "Object": 49
        },
        "Count": 1
      }
    },
    "parms": "200",
    "type": "~hp"
  }
}