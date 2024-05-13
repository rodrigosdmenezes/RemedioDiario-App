import React, { useState, useEffect } from "react";
import { View, Text, TouchableOpacity, FlatList, StyleSheet } from "react-native";
import Icon from 'react-native-vector-icons/MaterialCommunityIcons';
import axios from 'axios';
import { Alert } from 'react-native';

export default function ListaMedicamentos() {
  const [medicamentos, setMedicamentos] = useState([]);

  useEffect(() => {
    fetchMedicamentos();
  }, []);

  const fetchMedicamentos = async () => {
    try {
      const response = await axios.get('http://localhost:5122/api/Medicamentos');
      setMedicamentos(response.data);
    } catch (error) {
      console.log(error);
      Alert.alert("Erro", "Ocorreu um erro ao carregar os medicamentos. Por favor, tente novamente.");
    }
  };

  const handleEditar = (medicamentoId) => {

  };

  const handleExcluir = async (medicamentoId) => {
    const medicamentoParaExcluir = medicamentos.find(medicamento => medicamento.Id === medicamentoId);
    if (!medicamentoParaExcluir) {
      Alert.alert("Erro", "Medicamento não encontrado.");
      return;
    }
  
    try {
      const response = await axios.delete(`http://localhost:5122/api/Medicamentos/${encodeURIComponent(medicamentoId)}`);
      if (response.status === 200) {
        Alert.alert("Sucesso", "Medicamento excluído com sucesso.");
        fetchMedicamentos();
      } else {
        Alert.alert("Erro", "Erro ao excluir medicamento. Por favor, tente novamente.");
      }
    } catch (error) {
      console.log(error);
      Alert.alert("Erro", "Ocorreu um erro ao excluir medicamento. Por favor, tente novamente.");
    }
  };

  const renderItem = ({ item  }) => (
    <View style={styles.itemContainer}>
      <View style={styles.itemContent}>
        <Icon name="medical-bag" size={30} color="#777" style={styles.icon} />
        <View>
          <Text style={styles.itemTitle}>Nome: {item.Nome}</Text>
          <Text style={styles.itemDescription}>Descrição: {item.Descricao}</Text>
          <Text style={styles.itemDescription}>Quantidade: {item.Quantidade}</Text>
          <Text style={styles.itemDescription}>Tipo: {item.Tipo}</Text>
          <Text style={styles.itemDescription}>Data: {item.Data}</Text>
          <Text style={styles.itemDescription}>Hora: {item.Hora}</Text>
        </View>
      </View>
      <View style={styles.itemActions}>
        <TouchableOpacity style={styles.actionButton} onPress={() => handleEditar(item.Id)}>
          <Icon name="pencil" size={20} color="#777" />
        </TouchableOpacity>
        <TouchableOpacity style={styles.actionButton} onPress={() => handleExcluir(item.Id)}>
          <Icon name="delete" size={20} color="#777" />
        </TouchableOpacity>
      </View>
    </View>
  );

  return (
    <View style={styles.container}>
      <Text style={styles.title}>Remédios Cadastrados</Text>
      <FlatList
        data={medicamentos}
        renderItem={renderItem}
        keyExtractor={(item, index) => index.toString()}// Corrigindo o keyExtractor
      />
    </View>
  );
}

const styles = StyleSheet.create({
  container: {
    flex: 1,
    backgroundColor: "#FFF",
    padding: 20,
  },
  title: {
    fontSize: 24,
    fontWeight: "bold",
    marginBottom: 20,
  },
  itemContainer: {
    flexDirection: 'row',
    alignItems: 'center',
    justifyContent: 'space-between',
    borderBottomWidth: 1,
    borderColor: '#DDD',
    paddingVertical: 10,
  },
  itemContent: {
    flexDirection: 'row',
    alignItems: 'center',
  },
  icon: {
    marginRight: 10,
  },
  itemTitle: {
    fontSize: 18,
    fontWeight: 'bold',
  },
  itemDescription: {
    fontSize: 14,
    color: '#777',
  },
  itemActions: {
    flexDirection: 'row',
    alignItems: 'center',
  },
  actionButton: {
    marginLeft: 10,
  },
});
