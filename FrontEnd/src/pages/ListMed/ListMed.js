import React, { useState, useEffect } from "react";
import { View, Text, TouchableOpacity, FlatList, StyleSheet, TextInput } from "react-native";
import Icon from "react-native-vector-icons/MaterialCommunityIcons";
import axios from "axios";
import { Alert } from "react-native";

export default function ListaMedicamentos() {
  const [medicamentos, setMedicamentos] = useState([]);
  const [editingItemId, setEditingItemId] = useState(null);
  const [editedFields, setEditedFields] = useState({});

  useEffect(() => {
    fetchMedicamentos();
  }, []);

  const fetchMedicamentos = async () => {
    try {
      const response = await axios.get("http://localhost:5122/api/Medicamentos");
      if (response.status === 200) {
        setMedicamentos(response.data);
      } else {
        Alert.alert(
          "Erro",
          "Erro ao carregar os medicamentos. Por favor, tente novamente."
        );
      }
    } catch (error) {
      console.log(error);
      Alert.alert(
        "Erro",
        "Ocorreu um erro ao carregar os medicamentos. Por favor, tente novamente."
      );
    }
  };

  const handleEditItem = (item) => {
    setEditingItemId(item.id);
    setEditedFields({
      nome: item.nome,
      descricao: item.descricao,
      quantidade: item.quantidade,
      tipo: item.tipo,
      data: item.data,
      hora: item.hora,
    });
  };

  const handleSaveEdit = async (medicamentoId) => {
    try {
      const response = await axios.put(`http://localhost:5122/api/Medicamentos/${medicamentoId}`, editedFields);
      if (response.status === 200) {
        Alert.alert("Sucesso", "Medicamento atualizado com sucesso.");
        fetchMedicamentos();
        setEditingItemId(null);
        setEditedFields({});
      } else {
        Alert.alert("Erro", "Erro ao atualizar medicamento. Por favor, tente novamente.");
      }
    } catch (error) {
      console.log(error);
      Alert.alert("Erro", "Ocorreu um erro ao atualizar medicamento. Por favor, tente novamente.");
    }
  };

  const handleCancelEdit = () => {
    setEditingItemId(null);
    setEditedFields({});
  };

  const handleDeleteItem = async (medicamentoId) => {
    try {
      const response = await axios.delete(`http://localhost:5122/api/Medicamentos/${medicamentoId},`, editedFields);
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

  const renderItem = ({ item }) => (
    <View style={styles.itemContainer}>
      <View style={styles.itemContent}>
        <Icon name="medical-bag" size={30} color="#000000" style={styles.icon} />
        <View>
          {editingItemId === item.id ? (
            <View>
              <TextInput
                style={styles.input}
                placeholder="Nome"
                value={editedFields.nome}
                onChangeText={(text) => setEditedFields({ ...editedFields, nome: text })}
              />
              <TextInput
                style={styles.input}
                placeholder="Descrição"
                value={editedFields.descricao}
                onChangeText={(text) => setEditedFields({ ...editedFields, descricao: text })}
              />
              <TextInput
                style={styles.input}
                placeholder="Quantidade"
                value={editedFields.quantidade}
                onChangeText={(text) => setEditedFields({ ...editedFields, quantidade: text })}
              />
              <TextInput
                style={styles.input}
                placeholder="Tipo"
                value={editedFields.tipo}
                onChangeText={(text) => setEditedFields({ ...editedFields, tipo: text })}
              />
              <TextInput
                style={styles.input}
                placeholder="Data"
                value={editedFields.data}
                onChangeText={(text) => setEditedFields({ ...editedFields, data: text })}
              />
              <TextInput
                style={styles.input}
                placeholder="Hora"
                value={editedFields.hora}
                onChangeText={(text) => setEditedFields({ ...editedFields, hora: text })}
              />
              <TouchableOpacity onPress={() => handleSaveEdit(item.id)}>
                <Text>Salvar</Text>
              </TouchableOpacity>
              <TouchableOpacity onPress={handleCancelEdit}>
                <Text>Cancelar</Text>
              </TouchableOpacity>
            </View>
          ) : (
            <View>
              <Text style={styles.itemDescription}>Nome: {item.nome}</Text>
              <Text style={styles.itemDescription}>Descrição: {item.descricao}</Text>
              <Text style={styles.itemDescription}>Quantidade: {item.quantidade}</Text>
              <Text style={styles.itemDescription}>Tipo: {item.tipo}</Text>
              <Text style={styles.itemDescription}>Data: {item.data}</Text>
              <Text style={styles.itemDescription}>Hora: {item.hora}</Text>
            </View>
          )}
        </View>
      </View>
      <View style={styles.itemActions}>
        <TouchableOpacity
          style={styles.actionButton}
          onPress={() => handleEditItem(item)}
        >
          <Icon name="pencil" size={20} color="#000000" />
        </TouchableOpacity>
        <TouchableOpacity
          style={styles.actionButton}
          onPress={() => handleDeleteItem(item.id)}
        >
          <Icon name="delete" size={20} color="#000000" />
        </TouchableOpacity>
      </View>
    </View>
  );

  return (
    <View style={styles.container}>
      <View style={styles.containerHeader}>
        <Text style={styles.message}>Remédios Cadastrados</Text>
      </View>
      <FlatList
        data={medicamentos}
        renderItem={renderItem}
        keyExtractor={(item, index) => index.toString()}
        style={styles.flatList}
      />
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
  flatList: {
    backgroundColor: "#FFF",
    paddingHorizontal: 20,
    paddingTop: 20,
    borderTopLeftRadius: 25,
    borderTopRightRadius: 25,
  },
  itemContainer: {
    flexDirection: "row",
    alignItems: "center",
    justifyContent: "space-between",
    borderWidth: 1,
    borderColor: "#000000",
    borderRadius: 10,
    padding: 10,
    marginBottom: 10,
    backgroundColor: "#FFF",
  },
  itemContent: {
    flexDirection: "row",
    alignItems: "center",
  },
  icon: {
    marginRight: 10,
  },
  itemDescription: {
    fontSize: 13,
    color: "#000000",
  },
  itemActions: {
    flexDirection: "row",
    alignItems: "center",
  },
  actionButton: {
    marginLeft: 10,
  },
  input: {
    borderBottomWidth: 1,
    marginBottom: 5,
    padding: 5,
  },
});
