{
  "object 0": {
    "ID": 5,
    "gui": {
      "pos_x": "426",
      "pos_y": "373.42"
    },
    "outputs": {
      "output 0": {
        "Count": 0
      }
    },
    "parms": "",
    "type": ".f"
  },
  "object 1": {
    "ID": 1,
    "gui": {
      "pos_x": "195",
      "pos_y": "393.42"
    },
    "parms": "",
    "type": "~dac"
  },
  "object 2": {
    "ID": 2,
    "gui": {
      "pos_x": "195",
      "pos_y": "274.42"
    },
    "outputs": {
      "output 0": {
        "0": {
          "Inlet": 0,
          "Object": 6
        },
        "Count": 1
      }
    },
    "parms": "200",
    "type": "~sine"
  },
  "object 3": {
    "ID": 3,
    "gui": {
      "pos_x": "359",
      "pos_y": "306.42"
    },
    "outputs": {
      "output 0": {
        "0": {
          "Inlet": 1,
          "Object": 6
        },
        "1": {
          "Inlet": 0,
          "Object": 5
        },
        "Count": 2
      }
    },
    "parms": "volume",
    "type": ".r"
  },
  "object 4": {
    "ID": 6,
    "gui": {
      "pos_x": "195",
      "pos_y": "333.42"
    },
    "outputs": {
      "output 0": {
        "0": {
          "Inlet": 0,
          "Object": 1
        },
        "Count": 1
      }
    },
    "parms": "0",
    "type": "~*"
  },
  "object 5": {
    "ID": 4,
    "gui": {
      "height": "24.99998",
      "pos_x": "411",
      "pos_y": "243.42",
      "width": "150"
    },
    "outputs": {
      "output 0": {
        "0": {
          "Inlet": 0,
          "Object": 3
        },
        "Count": 1
      }
    },
    "parms": "",
    "type": ".slider"
  }
}