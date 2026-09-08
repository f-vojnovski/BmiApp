export interface BmiReadRecordDto {
    weight: number,
    height: number,
    bmi: number
}

export interface BmiWriteRecordDto {
    email: string,
    weight: number,
    height: number,
    bmi: number
}