{
  "object 0": {
    "ID": 78,
    "gui": {
      "height": "52",
      "pos_x": "341",
      "pos_y": "99",
      "width": "75"
    },
    "parms": "right inlet changes speed",
    "type": ".text"
  },
  "object 1": {
    "ID": 79,
    "gui": {
      "height": "65",
      "pos_x": "102",
      "pos_y": "146",
      "width": "78"
    },
    "parms": "metro outputs a trigger, not a value",
    "type": ".text"
  },
  "object 2": {
    "ID": 76,
    "gui": {
      "pos_x": "227",
      "pos_y": "151"
    },
    "outputs": {
      "output 0": {
        "0": {
          "Inlet": 1,
          "Object": 72
        },
        "Count": 1
      }
    },
    "parms": "100",
    "type": ".+"
  },
  "object 3": {
    "ID": 71,
    "gui": {
      "height": "25",
      "pos_x": "54",
      "pos_y": "39",
      "width": "25"
    },
    "outputs": {
      "output 0": {
        "0": {
          "Inlet": 0,
          "Object": 72
        },
        "Count": 1
      }
    },
    "parms": "",
    "type": ".t"
  },
  "object 4": {
    "ID": 77,
    "gui": {
      "height": "48",
      "pos_x": "100",
      "pos_y": "37",
      "width": "79"
    },
    "parms": "start/stop metro with toggle",
    "type": ".text"
  },
  "object 5": {
    "ID": 72,
    "gui": {
      "pos_x": "54",
      "pos_y": "91"
    },
    "outputs": {
      "output 0": {
        "0": {
          "Inlet": 0,
          "Object": 73
        },
        "Count": 1
      }
    },
    "parms": "1000",
    "type": ".metro"
  },
  "object 6": {
    "ID": 73,
    "gui": {
      "height": "25",
      "pos_x": "54",
      "pos_y": "144",
      "width": "25"
    },
    "outputs": {
      "output 0": {
        "Count": 0
      }
    },
    "parms": "",
    "type": ".b"
  },
  "object 7": {
    "ID": 74,
    "gui": {
      "height": "25",
      "pos_x": "227",
      "pos_y": "53",
      "width": "150"
    },
    "outputs": {
      "output 0": {
        "0": {
          "Inlet": 0,
          "Object": 75
        },
        "Count": 1
      }
    },
    "parms": "",
    "type": ".slider"
  },
  "object 8": {
    "ID": 75,
    "gui": {
      "pos_x": "227",
      "pos_y": "106"
    },
    "outputs": {
      "output 0": {
        "0": {
          "Inlet": 0,
          "Object": 76
        },
        "Count": 1
      }
    },
    "parms": "1000",
    "type": ".*"
  }
}