import React, { useState } from "react";
import { View, Text, TextInput, TouchableOpacity, StyleSheet, Button, Platform } from "react-native";
import Icon from 'react-native-vector-icons/MaterialCommunityIcons';
import axios from 'axios';
import { Alert } from 'react-native';
import DateTimePicker from '@react-native-community/datetimepicker';

export default function Menu({ navigation }) {
  const [name, setName] = useState("");
  const [description, setDescription] = useState("");
  const [quantity, setQuantity] = useState("");
  const [selectedType, setSelectedType] = useState("");
  const [selectedDate, setSelectedDate] = useState(new Date());
  const [showDatePicker, setShowDatePicker] = useState(false);
  const [selectedTime, setSelectedTime] = useState("");
  const [showTimePicker, setShowTimePicker] = useState(false);

  const handleSave = async () => {
    try {
      const response = await axios.post('http://localhost:5122/api/Medicamentos', {
        Nome: name,
        Descricao: description,
        Quantidade: quantity,
        Tipo: selectedType,
        Data: selectedDate.toLocaleDateString('pt-BR'), // Alterado para formato brasileiro
        Hora: selectedTime
      });
      if (response.status === 200) {
        Alert.alert("Sucesso", "Medicamento cadastrado com sucesso.");
      } else {
        Alert.alert("Erro", "Erro ao cadastrar medicamento. Por favor, tente novamente.");
      }
    } catch (error) {
      console.error(error);
      Alert.alert("Erro", "Ocorreu um erro ao cadastrar medicamento. Por favor, tente novamente.");
    }
  };

  const onChangeDate = (event, selectedDate) => {
    setShowDatePicker(false);
    if (selectedDate) {
      setSelectedDate(selectedDate);
    }
  };

  const onChangeTime = (event, selectedTime) => {
    setShowTimePicker(false);
    if (selectedTime) {
      const hours = selectedTime.getHours();
      const minutes = selectedTime.getMinutes();
      setSelectedTime(`${hours}:${minutes < 10 ? '0' + minutes : minutes}`);
    }
  };

  return (
    <View style={styles.container}>
      <View style={styles.containerHeader}>
        <Text style={styles.message}>Cadastrar Medicamento</Text>
      </View>

      <View style={styles.containerForm}>
        <Text style={styles.title}>Nome</Text>
        <View style={styles.inputContainer}>
          <Icon name="medical-bag" size={20} color="#777" style={styles.icon} />
          <TextInput
            style={styles.input}
            placeholder="Digite o nome"
            value={name}
            onChangeText={setName}
          />
        </View>

        <Text style={styles.title}>Descrição</Text>
        <View style={styles.inputContainer}>
          <Icon name="pencil" size={20} color="#777" style={styles.icon} />
          <TextInput
            style={styles.input}
            placeholder="Digite a descrição"
            value={description}
            onChangeText={setDescription}
          />
        </View>

        <Text style={styles.title}>Quantidade</Text>
        <View style={styles.inputContainer}>
          <Icon name="flask" size={20} color="#777" style={styles.icon} />
          <TextInput
            style={styles.input}
            placeholder="Digite a quantidade"
            value={quantity}
            onChangeText={setQuantity}
            keyboardType="numeric"
          />
        </View>

        <Text style={styles.title}>Tipo</Text>
        <View style={styles.inputContainer}>
          <Icon name="needle" size={20} color="#777" style={styles.icon} />
          <TextInput
            style={styles.input}
            placeholder="Informe o tipo, se é Gotas, Comprimido ou Vacina"
            value={selectedType}
            onChangeText={setSelectedType}
          />
        </View>

        <Text style={styles.title}>Data</Text>
        <View style={styles.inputContainer}>
          <TouchableOpacity onPress={() => setShowDatePicker(true)} style={styles.buttonContainer}>
            <Icon name="calendar" size={20} color="#777" style={styles.icon} />
            <Text style={styles.buttonText}>{selectedDate.toLocaleDateString('pt-BR')}</Text>
          </TouchableOpacity>
          {showDatePicker && (
            <DateTimePicker
              value={selectedDate}
              mode="date"
              display="default"
              onChange={onChangeDate}
            />
          )}
        </View>

        <Text style={styles.title}>Hora</Text>
        <View style={styles.inputContainer}>
          <TouchableOpacity onPress={() => setShowTimePicker(true)} style={styles.buttonContainer}>
            <Icon name="clock" size={20} color="#777" style={styles.icon} />
            <Text style={styles.buttonText}>{selectedTime}</Text>
          </TouchableOpacity>
          {showTimePicker && (
            <DateTimePicker
              value={new Date()}
              mode="time"
              display="default"
              onChange={onChangeTime}
            />
          )}
        </View>

        <View style={styles.buttonsContainer}>
          <TouchableOpacity style={[styles.button, styles.buttonSalvar]} onPress={handleSave}>
            <Text style={styles.buttonSave}>Salvar</Text>
          </TouchableOpacity>
          
          <TouchableOpacity style={[styles.button, styles.buttonVerMedicamentos]} onPress={() => navigation.navigate('ListMed')}>
            <Text style={styles.buttonSave}>Ver Medicamentos Cadastrados</Text>
          </TouchableOpacity>
        </View>
      </View>
    </View>
  );
}

const styles = StyleSheet.create({
  container: {
    flex: 1,
    backgroundColor: "#38A69D",
  },
  containerHeader: {
    marginTop: "14%",
    marginBottom: "8%",
    paddingHorizontal: "5%",
  },
  message: {
    fontSize: 28,
    fontWeight: "bold",
    color: "#FFF",
    marginTop: 28,
  },
  containerForm: {
    backgroundColor: "#FFF",
    flex: 1,
    borderTopLeftRadius: 25,
    borderTopRightRadius: 25,
    paddingHorizontal: "5%",
    paddingTop: 20,
  },
  title: {
    fontSize: 20,
    marginTop: 20,
  },
  inputContainer: {
    flexDirection: 'row',
    alignItems: 'center',
    justifyContent: 'space-between',
    borderBottomWidth: 1,
    borderBottomColor: '#ccc',
    marginBottom: 12,
  },
  input: {
    height: 40,
    fontSize: 16,
    flex: 1,
    paddingLeft: 10,
  },
  icon: {
    marginRight: 10,
  },
  buttonContainer: {
    flexDirection: 'row',
    alignItems: 'center',
  },
  buttonText: {
    color: "#777",
    fontSize: 16,
    marginLeft: 5,
  },
  buttonSave:{
    color: '#FFF'
  },
  buttonsContainer: {
    flexDirection: 'row',
    justifyContent: 'space-between',
    marginTop: 20,
  },
  button: {
    backgroundColor: "#38A69D",
    width: "48%",
    borderRadius: 4,
    paddingVertical: 8,
    justifyContent: "center",
    alignItems: "center",
  },
  buttonSalvar: {
    marginRight: '2%',
  },
  buttonVerMedicamentos: {
    marginLeft: '2%',
  },
});
