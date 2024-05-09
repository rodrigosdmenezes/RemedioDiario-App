import React, { useState } from "react";
import {
  View,
  Text,
  StyleSheet,
  TextInput,
  TouchableOpacity,
  Alert // Importar Alert
} from "react-native";
import Icon from "react-native-vector-icons/FontAwesome";
import { useNavigation } from "@react-navigation/native";
import axios from 'axios';

export default function Reset() {
  const navigation = useNavigation();
  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');
  const [showPassword, setShowPassword] = useState(false);

  const handleResetPassword = async () => {
    if (!email.trim()) {
      Alert.alert("Erro", "Por favor, insira um email válido.");
      return;
    }

    if (!password) {
      Alert.alert("Erro", "Por favor, insira uma senha.");
      return;
    }

    if (!isValidEmail(email)) {
      Alert.alert("Erro", "Por favor, insira um email válido.");
      return;
    }

    try {
      const response = await axios.post('http://localhost:5122/api/resetpassword', {
        email: email,
        newPassword: password
      });

      Alert.alert("Sucesso", response.data);

      navigation.navigate("SignIn");
    } catch (error) {
      if (error.response && error.response.status === 404){
        Alert.alert("Erro", "Email não encontrado")
      }
      else{
        Alert.alert("Erro", "Ocorreu um erro ao redefinir a senha. Por favor, tente novamente.");
      }
    }
  };

  const isValidEmail = (email) => {
    const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
    return emailRegex.test(email);
  };

  const handleTogglePasswordVisibility = () => {
    setShowPassword(!showPassword);
  };

  return (
    <View style={styles.container}>
      <View style={styles.containerHeader}>
        <Text style={styles.message}>Resete sua senha!</Text>
      </View>

      <View style={styles.containerForm}>
        <Text style={styles.title}>Email</Text>
        <TextInput
          placeholder="Digite email cadastrado"
          style={styles.input}
          value={email}
          onChangeText={setEmail}
          keyboardType="email-address"
          autoCapitalize="none"
        />

        <Text style={styles.title}>Nova Senha</Text>
        <View style={styles.passwordContainer}>
          <TextInput
            placeholder="Digite senha cadastrada"
            style={[styles.input, { flex: 1 }]}
            value={password}
            onChangeText={setPassword}
            secureTextEntry={!showPassword}
          />
          <TouchableOpacity onPress={handleTogglePasswordVisibility}>
            <Icon
              name={showPassword ? "eye-slash" : "eye"}
              size={20}
              color="#777"
            />
          </TouchableOpacity>
        </View>

        <TouchableOpacity style={styles.button} onPress={handleResetPassword}>
          <Text style={styles.buttonText}>Alterar Senha</Text>
        </TouchableOpacity>
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
  input: {
    borderBottomWidth: 1,
    height: 40,
    marginBottom: 12,
    fontSize: 16,
  },
  button: {
    backgroundColor: "#38A69D",
    width: "100%",
    borderRadius: 4,
    paddingVertical: 8,
    marginTop: 20,
    justifyContent: "center",
    alignItems: "center",
  },
  buttonText: {
    color: "#FFF",
    fontSize: 18,
    fontWeight: "bold",
  },
  passwordContainer: {
    flexDirection: 'row',
    alignItems: 'center',
  },
});
