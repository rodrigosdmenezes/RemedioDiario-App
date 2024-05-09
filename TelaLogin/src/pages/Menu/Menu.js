import React, { useState } from "react";
import {
  View,
  Text,
  TextInput,
  TouchableOpacity,
  StyleSheet,
} from "react-native";
import { CheckBox } from "react-native-elements";
import { Calendar } from "react-native-calendars";

export default function Menu() {
  const [name, setName] = useState("");
  const [description, setDescription] = useState("");
  const [quantity, setQuantity] = useState("");
  const [types, setTypes] = useState([
    { label: "Comprimido", value: "comprimido", checked: false },
    { label: "Gotas", value: "gotas", checked: false },
  ]);
  const [date, setDate] = useState(new Date());
  const [showDatePicker, setShowDatePicker] = useState(false);
  const [time, setTime] = useState(new Date());
  const [showTimePicker, setShowTimePicker] = useState(false);

  const handleSave = () => {
    // Aqui você pode enviar os dados para o backend para salvar no banco de dados
    console.log("Nome:", name);
    console.log("Descrição:", description);
    console.log("Quantidade:", quantity);
    console.log("Tipos selecionados:");
    types.forEach((type) => {
      if (type.checked) {
        console.log("- " + type.label);
      }
    });
    console.log("Data:", date);
    console.log("Hora:", time);
  };

  const handleDateChange = (event, selectedDate) => {
    const currentDate = selectedDate || date;
    setShowDatePicker(false);
    setDate(currentDate);
  };

  const handleTimeChange = (event, selectedTime) => {
    const currentTime = selectedTime || time;
    setShowTimePicker(false);
    setTime(currentTime);
  };

  const handleCheckboxChange = (value) => {
    const updatedTypes = types.map((type) =>
      type.value === value ? { ...type, checked: !type.checked } : type
    );
    setTypes(updatedTypes);
  };

  return (
    <View style={styles.container}>
      <Text style={styles.title}>Cadastrar Medicamento</Text>
      <TextInput
        style={styles.input}
        placeholder="Nome"
        value={name}
        onChangeText={setName}
      />
      <TextInput
        style={styles.input}
        placeholder="Descrição"
        value={description}
        onChangeText={setDescription}
      />
      <TextInput
        style={styles.input}
        placeholder="Quantidade"
        value={quantity}
        onChangeText={setQuantity}
        keyboardType="numeric"
      />

      <Text>Selecione o tipo:</Text>
      {types.map((type) => (
        <View key={type.value} style={styles.checkboxContainer}>
          <Text style={styles.label}>{type.label}</Text>
          <CheckBox
            checked={type.checked}
            onPress={() => handleCheckboxChange(type.value)}
          />
        </View>
      ))}

      <TouchableOpacity
        style={styles.button}
        onPress={() => setShowDatePicker(true)}
      >
        <Text style={styles.buttonText}>Selecionar data</Text>
      </TouchableOpacity>
      {showDatePicker && (
        <Calendar
          testID="datePicker"
          current={date}
          onDayPress={(day) => {
            handleDateChange(null, new Date(day.timestamp));
          }}
        />
      )}
      <TouchableOpacity
        style={styles.button}
        onPress={() => setShowTimePicker(true)}
      >
        <Text style={styles.buttonText}>Selecionar hora</Text>
      </TouchableOpacity>
      {showTimePicker && (
        <Calendar
          testID="timePicker"
          current={time}
          onDayPress={(day) => {
            handleTimeChange(null, new Date(day.timestamp));
          }}
        />
      )}
      <TouchableOpacity style={styles.button} onPress={handleSave}>
        <Text style={styles.buttonText}>Salvar</Text>
      </TouchableOpacity>
    </View>
  );
}

const styles = StyleSheet.create({
  container: {
    flex: 1,
    padding: 20,
    backgroundColor: "#fff",
  },
  title: {
    fontSize: 20,
    fontWeight: "bold",
    marginBottom: 20,
  },
  input: {
    height: 40,
    borderColor: "#ccc",
    borderWidth: 1,
    borderRadius: 5,
    marginBottom: 15,
    paddingHorizontal: 10,
  },
  button: {
    backgroundColor: "#38A69D",
    paddingVertical: 12,
    borderRadius: 5,
    marginBottom: 15,
    alignItems: "center",
  },
  buttonText: {
    color: "#fff",
    fontSize: 16,
    fontWeight: "bold",
  },
  checkboxContainer: {
    flexDirection: "row",
    alignItems: "center",
    justifyContent: "space-between",
    marginBottom: 10,
  },
  label: {
    marginRight: 8,
  },
});
