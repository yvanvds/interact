{
  "object 0": {
    "ID": 43,
    "gui": {
      "pos_x": "40",
      "pos_y": "261"
    },
    "parms": "",
    "type": "~dac"
  },
  "object 1": {
    "ID": 40,
    "gui": {
      "pos_x": "40",
      "pos_y": "207"
    },
    "outputs": {
      "output 0": {
        "0": {
          "Inlet": 0,
          "Object": 43
        },
        "Count": 1
      }
    },
    "parms": "0",
    "type": "~*"
  },
  "object 2": {
    "ID": 42,
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
          "Object": 41
        },
        "Count": 1
      }
    },
    "parms": "",
    "type": ".slider"
  },
  "object 3": {
    "ID": 44,
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
          "Object": 40
        },
        "Count": 1
      }
    },
    "parms": "",
    "type": ".slider"
  },
  "object 4": {
    "ID": 38,
    "gui": {
      "pos_x": "40",
      "pos_y": "119"
    },
    "outputs": {
      "output 0": {
        "0": {
          "Inlet": 0,
          "Object": 40
        },
        "Count": 1
      }
    },
    "parms": "200",
    "type": "~lp"
  },
  "object 5": {
    "ID": 46,
    "gui": {
      "height": "25",
      "pos_x": "163",
      "pos_y": "182",
      "width": "150"
    },
    "parms": "Set Volume",
    "type": ".text"
  },
  "object 6": {
    "ID": 41,
    "gui": {
      "pos_x": "158",
      "pos_y": "115"
    },
    "outputs": {
      "output 0": {
        "0": {
          "Inlet": 1,
          "Object": 38
        },
        "Count": 1
      }
    },
    "parms": "1000",
    "type": ".*"
  },
  "object 7": {
    "ID": 45,
    "gui": {
      "height": "25",
      "pos_x": "159",
      "pos_y": "43",
      "width": "150"
    },
    "parms": "Set lowpass frequency",
    "type": ".text"
  },
  "object 8": {
    "ID": 39,
    "gui": {
      "pos_x": "40",
      "pos_y": "70"
    },
    "outputs": {
      "output 0": {
        "0": {
          "Inlet": 0,
          "Object": 38
        },
        "Count": 1
      }
    },
    "parms": "",
    "type": "~noise"
  }
}